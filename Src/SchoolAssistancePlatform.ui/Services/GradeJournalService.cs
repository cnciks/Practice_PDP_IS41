using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Data;

namespace SchoolAssistancePlatform.UI.Services;

public class GradeJournalService(SAPDbContext context)
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Возвращает уникальные даты уроков по фильтрам, отсортированные по возрастанию.
	/// </summary>
	public async Task<List<DateTime>> GetLessonDatesAsync(
		long klassID, long? predmetID, DateTime startDate, DateTime endDate)
	{
		var query = _context.ZhurnalUspevaemosti
			.Include(j => j.Raspisanie)
			.Where(j => j.Raspisanie.KlassID == klassID
				&& j.DataUroka.Date >= startDate.Date
				&& j.DataUroka.Date <= endDate.Date);

		if(predmetID.HasValue)
			query = query.Where(j => j.Raspisanie.PredmetID == predmetID.Value);

		return await query
			.Select(j => j.DataUroka.Date)
			.Distinct()
			.OrderBy(d => d)
			.ToListAsync();
	}

	/// <summary>
	/// Возвращает учеников класса с оценками по каждой дате.
	/// </summary>
	public async Task<List<StudentJournalDto>> GetStudentsWithGradesAsync(
		long klassID, long? predmetID, DateTime startDate, DateTime endDate)
	{
		var students = await _context.Uchenik
			.Where(u => u.KlassID == klassID)
			.OrderBy(u => u.Familiia)
			.ToListAsync();

		var journalQuery = _context.ZhurnalUspevaemosti
			.Include(j => j.Raspisanie)
			.AsNoTracking();
			//.Where(j => j.Raspisanie.KlassID == klassID
			//	&& j.DataUroka.Date >= startDate.Date
			//	&& j.DataUroka.Date <= endDate.Date);

		if(predmetID.HasValue)
			journalQuery = journalQuery.Where(j => j.Raspisanie.PredmetID == predmetID.Value);

		var allRecords = await journalQuery.ToListAsync();

		var result = new List<StudentJournalDto>();
		foreach(var student in students)
		{
			var gradesByDate = allRecords
				.Where(j => j.UchenikID == student.UchenikID)
				.GroupBy(j => j.DataUroka.Date)
				.ToDictionary(
					g => g.Key,
					g => g.OrderByDescending(j => j.ZapisID).First());

			var validGrades = allRecords
				.Where(j => j.UchenikID == student.UchenikID && j.Ocenka > 0)
				.Select(j => j.Ocenka)
				.ToList();

			result.Add(new StudentJournalDto
			{
				StudentID    = student.UchenikID,
				StudentName  = $"{student.Familiia} {student.Imya} {student.Otchestvo}".Trim(),
				GradesByDate = gradesByDate,
				AverageGrade = validGrades.Count > 0 ? validGrades.Average() : 0,
			});
		}

		return result;
	}

	/// <summary>
	/// Сохраняет или обновляет оценку ученика за конкретную дату урока.
	/// Если ocenka == 0 и запись существует — удаляет её.
	/// </summary>
	public async Task UpsertGradeAsync(
		long studentID, long raspisanieID, DateTime dataUroka, int ocenka)
	{
		var existing = await _context.ZhurnalUspevaemosti
			.FirstOrDefaultAsync(j =>
				j.UchenikID      == studentID &&
				j.RaspisanieID   == raspisanieID &&
				j.DataUroka.Date == dataUroka.Date);

		if(existing is not null)
		{
			if(ocenka == 0)
				_context.ZhurnalUspevaemosti.Remove(existing);
			else
			{
				existing.Ocenka = ocenka;
				_context.ZhurnalUspevaemosti.Update(existing);
			}
		}
		else if(ocenka > 0)
		{
			await _context.ZhurnalUspevaemosti.AddAsync(new ZhurnalUspevaemostiEntity
			{
				UchenikID     = studentID,
				RaspisanieID  = raspisanieID,
				DataUroka     = dataUroka,
				Ocenka        = ocenka,
				TipOcenki     = "Текущая",
				Poseschaemost = true,
			});
		}

		await _context.SaveChangesAsync();
	}

	public async Task<IEnumerable<UchebniyPredmetDto>> GetSubjectsByKlassAsync(long klassID)
	{
		var predmetIDs = await _context.Raspisanie
			.Where(r => r.KlassID == klassID)
			.Select(r => r.PredmetID)
			.Distinct()
			.ToListAsync();

		var predmety = await _context.UchebniyPredmet
			.Where(p => predmetIDs.Contains(p.PredmetID))
			.OrderBy(p => p.Nazvanie)
			.ToListAsync();

		return predmety.Select(p => new UchebniyPredmetDto
		{
			PredmetID    = p.PredmetID,
			Nazvanie     = p.Nazvanie,
			Sokrashenie  = p.Sokrashenie,
			ChasovNedelyu = p.ChasovNedelyu,
		});
	}

	public async Task<double> GetStudentAverageGradeAsync(long studentID)
	{
		var grades = await _context.ZhurnalUspevaemosti
			.Where(j => j.UchenikID == studentID && j.Ocenka > 0)
			.Select(j => j.Ocenka)
			.ToListAsync();

		return grades.Count > 0 ? grades.Average() : 0;
	}
}

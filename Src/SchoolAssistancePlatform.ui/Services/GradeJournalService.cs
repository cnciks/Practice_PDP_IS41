using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.UI.Data;

namespace SchoolAssistancePlatform.UI.Services;

public class GradeJournalService
{
	private readonly SAPDbContext _context;
	private readonly IMapper _mapper;

	public GradeJournalService(SAPDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<List<StudentGradeDto>> GetStudentsWithGradesAsync(long klassID, DateTime startDate, DateTime endDate)
	{
		var students = await _context.Uchenik
			.Include(u => u.Klass)
			.Where(u => u.KlassID == klassID)
			.OrderBy(u => u.Familiia)
			.ToListAsync();

		var raspisanie = await _context.Raspisaniya
			.Where(r => r.KlassID == klassID)
			.ToListAsync();

		var result = new List<StudentGradeDto>();

		foreach(var student in students)
		{
			var studentDto = new StudentGradeDto
			{
				StudentID = student.UchenikID,
				StudentName = $"{student.Familiia} {student.Imya} {student.Otchestvo}".Trim(),
				Grades = new List<int?>(),
				AverageGrade = 0
			};

			foreach(var lesson in raspisanie)
			{
				var journal = await _context.ZhurnalUspevaemosti
					.FirstOrDefaultAsync(j => j.UchenikID == student.UchenikID &&
											   j.RaspisanieID == lesson.RaspisanieID &&
											   j.DataUroka.Date >= startDate.Date &&
											   j.DataUroka.Date <= endDate.Date);

				studentDto.Grades.Add(journal?.Ocenka);
			}

			studentDto.AverageGrade = await GetStudentAverageGradeAsync(student.UchenikID);
			result.Add(studentDto);
		}

		return result;
	}

	public async Task<List<DateTime>> GetLessonDatesAsync(long klassID, DateTime startDate, DateTime endDate)
	{
		var dates = await _context.ZhurnalUspevaemosti
			.Where(j => j.Raspisanie.KlassID == klassID &&
						j.DataUroka.Date >= startDate.Date &&
						j.DataUroka.Date <= endDate.Date)
			.Select(j => j.DataUroka.Date)
			.Distinct()
			.OrderBy(d => d)
			.ToListAsync();

		return dates;
	}

	public async Task UpdateGradeAsync(long studentID, long raspisanieID, int grade)
	{
		var journal = await _context.ZhurnalUspevaemosti
			.FirstOrDefaultAsync(j => j.UchenikID == studentID && j.RaspisanieID == raspisanieID);

		if(journal != null)
		{
			journal.Ocenka = grade;
			_context.ZhurnalUspevaemosti.Update(journal);
		}
		else
		{
			var newJournal = new ZhurnalUspevaemostiEntity
			{
				UchenikID = studentID,
				RaspisanieID = raspisanieID,
				DataUroka = DateTime.Now,
				Ocenka = grade,
				TipOcenki = "Текущая",
				Poseschaemost = true
			};
			await _context.ZhurnalUspevaemosti.AddAsync(newJournal);
		}

		await _context.SaveChangesAsync();
	}

	public async Task<double> GetStudentAverageGradeAsync(long studentID)
	{
		var grades = await _context.ZhurnalUspevaemosti
			.Where(j => j.UchenikID == studentID && j.Ocenka > 0)
			.Select(j => j.Ocenka)
			.ToListAsync();

		if(!grades.Any())
			return 0;

		return grades.Average();
	}

	public async Task SaveStudentNameAsync(long studentID, string newName)
	{
		var student = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == studentID);

		if(student != null)
		{
			var nameParts = newName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			student.Familiia = nameParts.Length > 0 ? nameParts[0] : student.Familiia;
			student.Imya = nameParts.Length > 1 ? nameParts[1] : student.Imya;
			student.Otchestvo = nameParts.Length > 2 ? nameParts[2] : student.Otchestvo;

			_context.Uchenik.Update(student);
			await _context.SaveChangesAsync();
		}
	}

	public async Task RemoveStudentAsync(long studentID)
	{
		var student = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == studentID);

		if(student != null)
		{
			_context.Uchenik.Remove(student);
			await _context.SaveChangesAsync();
		}
	}
}

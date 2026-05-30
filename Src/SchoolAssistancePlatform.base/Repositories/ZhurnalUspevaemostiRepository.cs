using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с журналом успеваемости (оценки и посещаемость учеников)
/// </summary>
public sealed class ZhurnalUspevaemostiRepository(SAPDbContext context) : IZhurnalUspevaemostiRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новую запись успеваемости (оценка и/или посещаемость)
	/// </summary>
	/// <param name="dto">Данные для создания записи</param>
	/// <returns>Созданный DTO записи успеваемости с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Расписание не найдено
	/// - Ученик не найден
	/// - Ученик не принадлежит классу, для которого составлено расписание
	/// </exception>
	public async Task<ZhurnalUspevaemostiDto> CreateAsync(ZhurnalUspevaemostiDto dto)
	{
		var raspisanie = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.FirstOrDefaultAsync(r => r.RaspisanieID == dto.RaspisanieID);
		if(raspisanie == null)
			throw new InvalidOperationException($"Расписание с ID {dto.RaspisanieID} не найдено");

		var uchenik = await _context.Uchenik
			.Include(u => u.Klass)
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		if(uchenik.KlassID != raspisanie.KlassID)
			throw new InvalidOperationException($"Ученик не принадлежит классу, для которого составлено расписание");

		var entity = dto.Adapt<ZhurnalUspevaemostiEntity>();

		await _context.ZhurnalUspevaemosti.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.ZapisID);
	}

	/// <summary>
	/// Получает запись успеваемости по идентификатору
	/// </summary>
	/// <param name="id">ID записи успеваемости</param>
	/// <returns>DTO записи успеваемости или null, если не найдена</returns>
	public async Task<ZhurnalUspevaemostiDto> GetByIdAsync(long id)
	{
		var entity = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.FirstOrDefaultAsync(z => z.ZapisID == id);

		return entity == null ? null : entity.Adapt<ZhurnalUspevaemostiDto>();
	}

	/// <summary>
	/// Получает все записи успеваемости, отсортированные по дате урока и фамилии ученика
	/// </summary>
	/// <returns>Коллекция DTO всех записей успеваемости</returns>
	public async Task<IEnumerable<ZhurnalUspevaemostiDto>> GetAllAsync()
	{
		var entities = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.OrderBy(z => z.DataUroka)
			.ThenBy(z => z.Uchenik.Familiia)
			.ToListAsync();

		return entities.Adapt<IEnumerable<ZhurnalUspevaemostiDto>>();
	}

	/// <summary>
	/// Получает записи успеваемости для указанного ученика
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Коллекция DTO записей успеваемости ученика, отсортированных по дате урока (сначала новые)</returns>
	public async Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByUchenikAsync(long uchenikID)
	{
		var entities = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.Where(z => z.UchenikID == uchenikID)
			.OrderByDescending(z => z.DataUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<ZhurnalUspevaemostiDto>>();
	}

	/// <summary>
	/// Получает записи успеваемости по расписанию занятия
	/// </summary>
	/// <param name="raspisanieID">ID расписания</param>
	/// <returns>Коллекция DTO записей успеваемости для указанного занятия</returns>
	public async Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByRaspisanieAsync(long raspisanieID)
	{
		var entities = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.Where(z => z.RaspisanieID == raspisanieID)
			.OrderBy(z => z.Uchenik.Familiia)
			.ToListAsync();

		return entities.Adapt<IEnumerable<ZhurnalUspevaemostiDto>>();
	}

	/// <summary>
	/// Получает записи успеваемости для указанного класса
	/// </summary>
	/// <param name="klassID">ID класса</param>
	/// <returns>Коллекция DTO записей успеваемости для всех учеников класса</returns>
	public async Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByKlassAsync(long klassID)
	{
		var entities = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.Where(z => z.Raspisanie.KlassID == klassID)
			.OrderBy(z => z.DataUroka)
			.ThenBy(z => z.Uchenik.Familiia)
			.ToListAsync();

		return entities.Adapt<IEnumerable<ZhurnalUspevaemostiDto>>();
	}

	/// <summary>
	/// Получает записи успеваемости за указанный диапазон дат
	/// </summary>
	/// <param name="startDate">Начальная дата</param>
	/// <param name="endDate">Конечная дата</param>
	/// <returns>Коллекция DTO записей успеваемости за указанный период</returns>
	public async Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		var entities = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.Klass)
			.Include(z => z.Raspisanie)
				.ThenInclude(r => r.UchebniyPredmet)
			.Include(z => z.Uchenik)
			.Where(z => z.DataUroka.Date >= startDate.Date && z.DataUroka.Date <= endDate.Date)
			.OrderBy(z => z.DataUroka)
			.ThenBy(z => z.Uchenik.Familiia)
			.ToListAsync();

		return entities.Adapt<IEnumerable<ZhurnalUspevaemostiDto>>();
	}

	/// <summary>
	/// Рассчитывает средний балл успеваемости ученика
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Средний балл (только по оценкам > 0 при посещаемости true)</returns>
	public async Task<double> GetAverageOcenkaByUchenikAsync(long uchenikID)
	{
		var ocenki = await _context.ZhurnalUspevaemosti
			.Where(z => z.UchenikID == uchenikID && z.Ocenka > 0 && z.Poseschaemost == true)
			.Select(z => z.Ocenka)
			.ToListAsync();

		if(!ocenki.Any())
			return 0;

		return ocenki.Average();
	}

	/// <summary>
	/// Обновляет существующую запись успеваемости
	/// </summary>
	/// <param name="id">ID обновляемой записи</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO записи успеваемости или null, если запись не найдена</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Расписание не найдено
	/// - Ученик не найден
	/// - Ученик не принадлежит классу, для которого составлено расписание
	/// </exception>
	public async Task<ZhurnalUspevaemostiDto> UpdateAsync(long id, ZhurnalUspevaemostiDto dto)
	{
		var existing = await _context.ZhurnalUspevaemosti
			.Include(z => z.Raspisanie)
			.Include(z => z.Uchenik)
			.FirstOrDefaultAsync(z => z.ZapisID == id);

		if(existing == null)
			return null;

		var raspisanie = await _context.Raspisaniya
			.FirstOrDefaultAsync(r => r.RaspisanieID == dto.RaspisanieID);
		if(raspisanie == null)
			throw new InvalidOperationException($"Расписание с ID {dto.RaspisanieID} не найдено");

		var uchenik = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		if(uchenik.KlassID != raspisanie.KlassID)
			throw new InvalidOperationException($"Ученик не принадлежит классу, для которого составлено расписание");

		dto.Adapt(existing);
		_context.ZhurnalUspevaemosti.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет запись успеваемости по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемой записи</param>
	/// <returns>true - запись удалена, false - запись не найдена</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.ZhurnalUspevaemosti
			.FirstOrDefaultAsync(z => z.ZapisID == id);

		if(entity == null)
			return false;

		_context.ZhurnalUspevaemosti.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование записи успеваемости
	/// </summary>
	/// <param name="id">ID записи</param>
	/// <returns>true - запись существует, false - не найдена</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.ZhurnalUspevaemosti
			.AnyAsync(z => z.ZapisID == id);
	}
}

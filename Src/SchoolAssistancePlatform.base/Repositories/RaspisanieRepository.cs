using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с расписанием занятий
/// </summary>
public sealed class RaspisaniyaRepository(SAPDbContext context) : IRaspisanieRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новую запись в расписании (урок)
	/// </summary>
	/// <param name="dto">Данные для создания расписания</param>
	/// <returns>Созданный DTO расписания с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Класс не найден
	/// - Предмет не найден
	/// - Сотрудник (преподаватель) не найден
	/// - Расписание на указанный день и урок для класса уже существует
	/// </exception>
	public async Task<RaspisanieDto> CreateAsync(RaspisanieDto dto)
	{
		var klass = await _context.Klassy
			.FirstOrDefaultAsync(k => k.KlassID == dto.KlassID);
		if(klass == null)
			throw new InvalidOperationException($"Класс с ID {dto.KlassID} не найден");

		var predmet = await _context.UchebniyPredmet
			.FirstOrDefaultAsync(p => p.PredmetID == dto.PredmetID);
		if(predmet == null)
			throw new InvalidOperationException($"Предмет с ID {dto.PredmetID} не найден");

		var sotrudnik = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.SotrudnikID);
		if(sotrudnik == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.SotrudnikID} не найден");

		var isDuplicate = await IsDuplicateScheduleAsync(dto.KlassID, dto.DenNedeli, dto.NomerUroka);
		if(isDuplicate)
			throw new InvalidOperationException($"Расписание для класса {klass.NomerKlassa} на {GetDayName(dto.DenNedeli)} {dto.NomerUroka} урок уже существует");

		var entity = dto.Adapt<RaspisanieEntity>();

		await _context.Raspisaniya.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.RaspisanieID);
	}

	/// <summary>
	/// Получает запись расписания по идентификатору
	/// </summary>
	/// <param name="id">ID записи расписания</param>
	/// <returns>DTO расписания или null, если не найдено</returns>
	public async Task<RaspisanieDto> GetByIdAsync(long id)
	{
		var entity = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.FirstOrDefaultAsync(r => r.RaspisanieID == id);

		return entity == null ? null : entity.Adapt<RaspisanieDto>();
	}

	/// <summary>
	/// Получает всё расписание, отсортированное по дню недели и номеру урока
	/// </summary>
	/// <returns>Коллекция DTO всех записей расписания</returns>
	public async Task<IEnumerable<RaspisanieDto>> GetAllAsync()
	{
		var entities = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.OrderBy(r => r.DenNedeli)
			.ThenBy(r => r.NomerUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<RaspisanieDto>>();
	}

	/// <summary>
	/// Получает расписание для указанного класса
	/// </summary>
	/// <param name="klassID">ID класса</param>
	/// <returns>Коллекция DTO записей расписания для класса</returns>
	public async Task<IEnumerable<RaspisanieDto>> GetByKlassAsync(long klassID)
	{
		var entities = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.Where(r => r.KlassID == klassID)
			.OrderBy(r => r.DenNedeli)
			.ThenBy(r => r.NomerUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<RaspisanieDto>>();
	}

	/// <summary>
	/// Получает расписание для указанного преподавателя
	/// </summary>
	/// <param name="sotrudnikID">ID сотрудника (преподавателя)</param>
	/// <returns>Коллекция DTO записей расписания преподавателя</returns>
	public async Task<IEnumerable<RaspisanieDto>> GetBySotrudnikAsync(long sotrudnikID)
	{
		var entities = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.Where(r => r.SotrudnikID == sotrudnikID)
			.OrderBy(r => r.DenNedeli)
			.ThenBy(r => r.NomerUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<RaspisanieDto>>();
	}

	/// <summary>
	/// Получает расписание на указанный день недели
	/// </summary>
	/// <param name="denNedeli">День недели (1-понедельник, ..., 7-воскресенье)</param>
	/// <returns>Коллекция DTO записей расписания на указанный день</returns>
	public async Task<IEnumerable<RaspisanieDto>> GetByDenNedeliAsync(long denNedeli)
	{
		var entities = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.Where(r => r.DenNedeli == denNedeli)
			.OrderBy(r => r.NomerUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<RaspisanieDto>>();
	}

	/// <summary>
	/// Получает расписание для указанного класса на указанный день недели
	/// </summary>
	/// <param name="klassID">ID класса</param>
	/// <param name="denNedeli">День недели</param>
	/// <returns>Коллекция DTO записей расписания класса на указанный день</returns>
	public async Task<IEnumerable<RaspisanieDto>> GetByKlassAndDenAsync(long klassID, long denNedeli)
	{
		var entities = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.Where(r => r.KlassID == klassID && r.DenNedeli == denNedeli)
			.OrderBy(r => r.NomerUroka)
			.ToListAsync();

		return entities.Adapt<IEnumerable<RaspisanieDto>>();
	}

	/// <summary>
	/// Обновляет существующую запись расписания
	/// </summary>
	/// <param name="id">ID обновляемой записи</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO расписания</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Класс не найден
	/// - Предмет не найден
	/// - Сотрудник (преподаватель) не найден
	/// - Расписание на указанный день и урок для класса уже существует (дубликат)
	/// </exception>
	public async Task<RaspisanieDto> UpdateAsync(long id, RaspisanieDto dto)
	{
		var existing = await _context.Raspisaniya
			.Include(r => r.Klass)
			.Include(r => r.UchebniyPredmet)
			.Include(r => r.Sotrudnik)
			.FirstOrDefaultAsync(r => r.RaspisanieID == id);

		if(existing == null)
			return null;

		var klass = await _context.Klassy
			.FirstOrDefaultAsync(k => k.KlassID == dto.KlassID);
		if(klass == null)
			throw new InvalidOperationException($"Класс с ID {dto.KlassID} не найден");

		var predmet = await _context.UchebniyPredmet
			.FirstOrDefaultAsync(p => p.PredmetID == dto.PredmetID);
		if(predmet == null)
			throw new InvalidOperationException($"Предмет с ID {dto.PredmetID} не найден");

		var sotrudnik = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.SotrudnikID);
		if(sotrudnik == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.SotrudnikID} не найден");

		var isDuplicate = await IsDuplicateScheduleAsync(dto.KlassID, dto.DenNedeli, dto.NomerUroka, id);
		if(isDuplicate)
			throw new InvalidOperationException($"Расписание для класса {klass.NomerKlassa} на {GetDayName(dto.DenNedeli)} {dto.NomerUroka} урок уже существует");

		dto.Adapt(existing);
		_context.Raspisaniya.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет запись расписания по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемой записи</param>
	/// <returns>true - запись удалена, false - запись не найдена</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.Raspisaniya
			.FirstOrDefaultAsync(r => r.RaspisanieID == id);

		if(entity == null)
			return false;

		_context.Raspisaniya.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование записи расписания
	/// </summary>
	/// <param name="id">ID записи</param>
	/// <returns>true - запись существует, false - не найдена</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.Raspisaniya
			.AnyAsync(r => r.RaspisanieID == id);
	}

	/// <summary>
	/// Проверяет наличие дубликата в расписании (одинаковый класс, день недели и номер урока)
	/// </summary>
	/// <param name="klassID">ID класса</param>
	/// <param name="denNedeli">День недели</param>
	/// <param name="nomerUroka">Номер урока</param>
	/// <param name="excludeId">ID записи для исключения из проверки (при обновлении)</param>
	/// <returns>true - дубликат существует, false - дубликатов нет</returns>
	public async Task<bool> IsDuplicateScheduleAsync(long klassID, long denNedeli, short nomerUroka, long? excludeId = null)
	{
		var query = _context.Raspisaniya
			.Where(r => r.KlassID == klassID && r.DenNedeli == denNedeli && r.NomerUroka == nomerUroka);

		if(excludeId.HasValue)
		{
			query = query.Where(r => r.RaspisanieID != excludeId.Value);
		}

		return await query.AnyAsync();
	}

	/// <summary>
	/// Возвращает название дня недели по его числовому коду
	/// </summary>
	/// <param name="denNedeli">Код дня недели (1-7)</param>
	/// <returns>Название дня недели</returns>
	private string GetDayName(long denNedeli)
	{
		return denNedeli switch
		{
			1 => "Понедельник",
			2 => "Вторник",
			3 => "Среда",
			4 => "Четверг",
			5 => "Пятница",
			6 => "Суббота",
			7 => "Воскресенье",
			_ => "Неизвестный день"
		};
	}
}

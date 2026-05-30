using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с приказами (распорядительными документами)
/// </summary>
public sealed class PrikazRepository(SAPDbContext context) : IPrikazRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новый приказ
	/// </summary>
	/// <param name="dto">Данные для создания приказа</param>
	/// <returns>Созданный DTO приказа с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Приказ с таким номером уже существует
	/// - Сотрудник (подписант) не найден
	/// </exception>
	public async Task<PrikazDto> CreateAsync(PrikazDto dto)
	{
		var isDuplicate = await IsDuplicateNomerPrikazaAsync(dto.NomerPrikaza);
		if(isDuplicate)
			throw new InvalidOperationException($"Приказ с номером {dto.NomerPrikaza} уже существует");

		var sotrudnik = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.SotrudnikID);
		if(sotrudnik == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.SotrudnikID} не найден");

		var entity = dto.Adapt<PrikazEntity>();

		await _context.Prikazy.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.PrikazID);
	}

	/// <summary>
	/// Получает приказ по идентификатору
	/// </summary>
	/// <param name="id">ID приказа</param>
	/// <returns>DTO приказа или null, если не найден</returns>
	public async Task<PrikazDto> GetByIdAsync(int id)
	{
		var entity = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.FirstOrDefaultAsync(p => p.PrikazID == id);

		return entity == null ? null : entity.Adapt<PrikazDto>();
	}

	/// <summary>
	/// Получает все приказы, отсортированные по дате (сначала новые)
	/// </summary>
	/// <returns>Коллекция DTO всех приказов</returns>
	public async Task<IEnumerable<PrikazDto>> GetAllAsync()
	{
		var entities = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.OrderByDescending(p => p.DataPrikaza)
			.ToListAsync();

		return entities.Adapt<IEnumerable<PrikazDto>>();
	}

	/// <summary>
	/// Получает приказы по типу
	/// </summary>
	/// <param name="tipPrikaza">Тип приказа (например, приказ о зачислении, переводе и т.д.)</param>
	/// <returns>Коллекция DTO приказов указанного типа</returns>
	public async Task<IEnumerable<PrikazDto>> GetByTipPrikazaAsync(string tipPrikaza)
	{
		var entities = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.Where(p => p.TipPrikaza == tipPrikaza)
			.OrderByDescending(p => p.DataPrikaza)
			.ToListAsync();

		return entities.Adapt<IEnumerable<PrikazDto>>();
	}

	/// <summary>
	/// Получает приказы по сотруднику (подписанту)
	/// </summary>
	/// <param name="sotrudnikID">ID сотрудника</param>
	/// <returns>Коллекция DTO приказов, подписанных указанным сотрудником</returns>
	public async Task<IEnumerable<PrikazDto>> GetBySotrudnikAsync(long sotrudnikID)
	{
		var entities = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.Where(p => p.SotrudnikID == sotrudnikID)
			.OrderByDescending(p => p.DataPrikaza)
			.ToListAsync();

		return entities.Adapt<IEnumerable<PrikazDto>>();
	}

	/// <summary>
	/// Получает приказы за указанный диапазон дат
	/// </summary>
	/// <param name="startDate">Начальная дата</param>
	/// <param name="endDate">Конечная дата</param>
	/// <returns>Коллекция DTO приказов за указанный период</returns>
	public async Task<IEnumerable<PrikazDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		var entities = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.Where(p => p.DataPrikaza.Date >= startDate.Date && p.DataPrikaza.Date <= endDate.Date)
			.OrderBy(p => p.DataPrikaza)
			.ToListAsync();

		return entities.Adapt<IEnumerable<PrikazDto>>();
	}

	/// <summary>
	/// Обновляет существующий приказ
	/// </summary>
	/// <param name="id">ID обновляемого приказа</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO приказа</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Приказ с таким номером уже существует (дубликат)
	/// - Сотрудник (подписант) не найден
	/// </exception>
	public async Task<PrikazDto> UpdateAsync(int id, PrikazDto dto)
	{
		var existing = await _context.Prikazy
			.Include(p => p.Sotrudnik)
			.FirstOrDefaultAsync(p => p.PrikazID == id);

		if(existing == null)
			return null;

		var isDuplicate = await IsDuplicateNomerPrikazaAsync(dto.NomerPrikaza, id);
		if(isDuplicate)
			throw new InvalidOperationException($"Приказ с номером {dto.NomerPrikaza} уже существует");

		var sotrudnik = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.SotrudnikID);
		if(sotrudnik == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.SotrudnikID} не найден");

		dto.Adapt(existing);
		_context.Prikazy.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет приказ по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого приказа</param>
	/// <returns>true - приказ удалён, false - приказ не найден</returns>
	public async Task<bool> DeleteAsync(int id)
	{
		var entity = await _context.Prikazy
			.FirstOrDefaultAsync(p => p.PrikazID == id);

		if(entity == null)
			return false;

		_context.Prikazy.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование приказа
	/// </summary>
	/// <param name="id">ID приказа</param>
	/// <returns>true - приказ существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(int id)
	{
		return await _context.Prikazy
			.AnyAsync(p => p.PrikazID == id);
	}

	/// <summary>
	/// Проверяет наличие дубликата по номеру приказа
	/// </summary>
	/// <param name="nomerPrikaza">Номер приказа</param>
	/// <param name="excludeId">ID приказа для исключения из проверки (при обновлении)</param>
	/// <returns>true - дубликат существует, false - дубликатов нет</returns>
	public async Task<bool> IsDuplicateNomerPrikazaAsync(string nomerPrikaza, int? excludeId = null)
	{
		var query = _context.Prikazy
			.Where(p => p.NomerPrikaza == nomerPrikaza);

		if(excludeId.HasValue)
		{
			query = query.Where(p => p.PrikazID != excludeId.Value);
		}

		return await query.AnyAsync();
	}
}

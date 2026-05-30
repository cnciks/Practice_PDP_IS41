using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью движения учеников (перевод, отчисление, восстановление)
/// </summary>
public sealed class DvizhenieUcheniakovRepository(SAPDbContext context) : IDvizhenieUcheniakovRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новую запись о движении ученика
	/// </summary>
	/// <param name="dto">Данные для создания</param>
	/// <returns>Созданный DTO с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">Ученик с указанным ID не найден</exception>
	public async Task<DvizhenieUcheniakovDto> CreateAsync(DvizhenieUcheniakovDto dto)
	{
		var uchenik = await _context.Uchenik
			.Include(u => u.Klass)
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		var entity = dto.Adapt<DvizhenieUcheniakovEntity>();

		await _context.DvizhenieUcheniakov.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.DvizhenieID);
	}

	/// <summary>
	/// Получает запись о движении ученика по идентификатору
	/// </summary>
	/// <param name="id">ID записи движения</param>
	/// <returns>DTO записи движения или null, если не найдена</returns>
	public async Task<DvizhenieUcheniakovDto> GetByIdAsync(long id)
	{
		var entity = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.FirstOrDefaultAsync(d => d.DvizhenieID == id);

		return entity == null ? null : entity.Adapt<DvizhenieUcheniakovDto>();
	}

	/// <summary>
	/// Получает все записи о движении учеников, отсортированные по дате изменения (сначала новые)
	/// </summary>
	/// <returns>Коллекция DTO всех записей движения</returns>
	public async Task<IEnumerable<DvizhenieUcheniakovDto>> GetAllAsync()
	{
		var entities = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.OrderByDescending(d => d.DataIzmeneniya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<DvizhenieUcheniakovDto>>();
	}

	/// <summary>
	/// Получает все записи о движении для конкретного ученика
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Коллекция DTO записей движения ученика, отсортированных по дате изменения</returns>
	public async Task<IEnumerable<DvizhenieUcheniakovDto>> GetByUchenikAsync(long uchenikID)
	{
		var entities = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(d => d.UchenikID == uchenikID)
			.OrderByDescending(d => d.DataIzmeneniya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<DvizhenieUcheniakovDto>>();
	}

	/// <summary>
	/// Получает записи о движении по типу движения
	/// </summary>
	/// <param name="tipDvizheniya">Тип движения (например, перевод, отчисление)</param>
	/// <returns>Коллекция DTO записей с указанным типом движения</returns>
	public async Task<IEnumerable<DvizhenieUcheniakovDto>> GetByTipDvizheniyaAsync(string tipDvizheniya)
	{
		var entities = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(d => d.TipDvizheniya == tipDvizheniya)
			.OrderByDescending(d => d.DataIzmeneniya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<DvizhenieUcheniakovDto>>();
	}

	/// <summary>
	/// Получает записи о движении за указанный диапазон дат
	/// </summary>
	/// <param name="startDate">Начальная дата</param>
	/// <param name="endDate">Конечная дата</param>
	/// <returns>Коллекция DTO записей движения за указанный период</returns>
	public async Task<IEnumerable<DvizhenieUcheniakovDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		var entities = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(d => d.DataIzmeneniya.Date >= startDate.Date && d.DataIzmeneniya.Date <= endDate.Date)
			.OrderBy(d => d.DataIzmeneniya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<DvizhenieUcheniakovDto>>();
	}

	/// <summary>
	/// Обновляет существующую запись о движении ученика
	/// </summary>
	/// <param name="id">ID обновляемой записи</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO записи движения</returns>
	/// <exception cref="InvalidOperationException">Ученик с указанным ID не найден</exception>
	public async Task<DvizhenieUcheniakovDto> UpdateAsync(long id, DvizhenieUcheniakovDto dto)
	{
		var existing = await _context.DvizhenieUcheniakov
			.Include(d => d.Uchenik)
				.ThenInclude(u => u.Klass)
			.FirstOrDefaultAsync(d => d.DvizhenieID == id);

		if(existing == null)
			return null;

		var uchenik = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		dto.Adapt(existing);
		_context.DvizhenieUcheniakov.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет запись о движении ученика по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемой записи</param>
	/// <returns>true - запись удалена, false - запись не найдена</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.DvizhenieUcheniakov
			.FirstOrDefaultAsync(d => d.DvizhenieID == id);

		if(entity == null)
			return false;

		_context.DvizhenieUcheniakov.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование записи о движении ученика
	/// </summary>
	/// <param name="id">ID записи</param>
	/// <returns>true - запись существует, false - не найдена</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.DvizhenieUcheniakov
			.AnyAsync(d => d.DvizhenieID == id);
	}
}

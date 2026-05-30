using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с финансовыми операциями (платежами учеников)
/// </summary>
public sealed class FinansyRepository(SAPDbContext context) : IFinansyRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новую финансовую операцию (платёж)
	/// </summary>
	/// <param name="dto">Данные для создания</param>
	/// <returns>Созданный DTO с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">Ученик с указанным ID не найден</exception>
	public async Task<FinansyDto> CreateAsync(FinansyDto dto)
	{
		var uchenik = await _context.Uchenik
			.Include(u => u.Klass)
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		var entity = dto.Adapt<FinansyEntity>();

		await _context.Finansy.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.PlatezhID);
	}

	/// <summary>
	/// Получает финансовую операцию по идентификатору
	/// </summary>
	/// <param name="id">ID платежа</param>
	/// <returns>DTO финансовой операции или null, если не найдена</returns>
	public async Task<FinansyDto> GetByIdAsync(long id)
	{
		var entity = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.FirstOrDefaultAsync(f => f.PlatezhID == id);

		return entity == null ? null : entity.Adapt<FinansyDto>();
	}

	/// <summary>
	/// Получает все финансовые операции, отсортированные по дате платежа (сначала новые)
	/// </summary>
	/// <returns>Коллекция DTO всех финансовых операций</returns>
	public async Task<IEnumerable<FinansyDto>> GetAllAsync()
	{
		var entities = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.OrderByDescending(f => f.DataPlatezha)
			.ToListAsync();

		return entities.Adapt<IEnumerable<FinansyDto>>();
	}

	/// <summary>
	/// Получает все финансовые операции для конкретного ученика
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Коллекция DTO финансовых операций ученика, отсортированных по дате платежа</returns>
	public async Task<IEnumerable<FinansyDto>> GetByUchenikAsync(long uchenikID)
	{
		var entities = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(f => f.UchenikID == uchenikID)
			.OrderByDescending(f => f.DataPlatezha)
			.ToListAsync();

		return entities.Adapt<IEnumerable<FinansyDto>>();
	}

	/// <summary>
	/// Получает финансовые операции по типу операции
	/// </summary>
	/// <param name="tipOperacii">Тип операции (Приход, Расход, Оплата, Возврат)</param>
	/// <returns>Коллекция DTO финансовых операций с указанным типом</returns>
	public async Task<IEnumerable<FinansyDto>> GetByTipOperaciiAsync(string tipOperacii)
	{
		var entities = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(f => f.TipOperacii == tipOperacii)
			.OrderByDescending(f => f.DataPlatezha)
			.ToListAsync();

		return entities.Adapt<IEnumerable<FinansyDto>>();
	}

	/// <summary>
	/// Получает финансовые операции за указанный диапазон дат
	/// </summary>
	/// <param name="startDate">Начальная дата</param>
	/// <param name="endDate">Конечная дата</param>
	/// <returns>Коллекция DTO финансовых операций за указанный период</returns>
	public async Task<IEnumerable<FinansyDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		var entities = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.Where(f => f.DataPlatezha.Date >= startDate.Date && f.DataPlatezha.Date <= endDate.Date)
			.OrderBy(f => f.DataPlatezha)
			.ToListAsync();

		return entities.Adapt<IEnumerable<FinansyDto>>();
	}

	/// <summary>
	/// Получает общую сумму всех операций для указанного ученика
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Общая сумма операций</returns>
	public async Task<decimal> GetTotalSumByUchenikAsync(long uchenikID)
	{
		return await _context.Finansy
			.Where(f => f.UchenikID == uchenikID)
			.SumAsync(f => f.Summa);
	}

	/// <summary>
	/// Рассчитывает баланс ученика (Приход/Оплата - Расход/Возврат)
	/// </summary>
	/// <param name="uchenikID">ID ученика</param>
	/// <returns>Текущий баланс ученика</returns>
	public async Task<decimal> GetBalanceByUchenikAsync(long uchenikID)
	{
		var operations = await _context.Finansy
			.Where(f => f.UchenikID == uchenikID)
			.ToListAsync();

		decimal balance = 0;
		foreach(var op in operations)
		{
			if(op.TipOperacii == "Приход" || op.TipOperacii == "Оплата")
				balance += op.Summa;
			else if(op.TipOperacii == "Расход" || op.TipOperacii == "Возврат")
				balance -= op.Summa;
		}

		return balance;
	}

	/// <summary>
	/// Обновляет существующую финансовую операцию
	/// </summary>
	/// <param name="id">ID обновляемой операции</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO финансовой операции</returns>
	/// <exception cref="InvalidOperationException">Ученик с указанным ID не найден</exception>
	public async Task<FinansyDto> UpdateAsync(long id, FinansyDto dto)
	{
		var existing = await _context.Finansy
			.Include(f => f.Uchenik)
				.ThenInclude(u => u.Klass)
			.FirstOrDefaultAsync(f => f.PlatezhID == id);

		if(existing == null)
			return null;

		var uchenik = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == dto.UchenikID);
		if(uchenik == null)
			throw new InvalidOperationException($"Ученик с ID {dto.UchenikID} не найден");

		dto.Adapt(existing);
		_context.Finansy.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет финансовую операцию по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемой операции</param>
	/// <returns>true - операция удалена, false - операция не найдена</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.Finansy
			.FirstOrDefaultAsync(f => f.PlatezhID == id);

		if(entity == null)
			return false;

		_context.Finansy.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование финансовой операции
	/// </summary>
	/// <param name="id">ID операции</param>
	/// <returns>true - операция существует, false - не найдена</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.Finansy
			.AnyAsync(f => f.PlatezhID == id);
	}
}

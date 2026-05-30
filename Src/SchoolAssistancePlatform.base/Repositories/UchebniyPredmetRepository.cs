using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с учебными предметами (дисциплинами)
/// </summary>
public sealed class UchebniyPredmetRepository(SAPDbContext context) : IUchebniyPredmetRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новый учебный предмет
	/// </summary>
	/// <param name="dto">Данные для создания предмета</param>
	/// <returns>Созданный DTO предмета с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">Предмет с таким сокращением уже существует</exception>
	public async Task<UchebniyPredmetDto> CreateAsync(UchebniyPredmetDto dto)
	{
		var isDuplicate = await IsDuplicateSokrashenieAsync(dto.Sokrashenie);
		if(isDuplicate)
			throw new InvalidOperationException($"Предмет с сокращением {dto.Sokrashenie} уже существует");

		var entity = dto.Adapt<UchebniyPredmetEntity>();

		await _context.UchebniyPredmet.AddAsync(entity);
		await _context.SaveChangesAsync();

		return entity.Adapt<UchebniyPredmetDto>();
	}

	/// <summary>
	/// Получает учебный предмет по идентификатору
	/// </summary>
	/// <param name="id">ID предмета</param>
	/// <returns>DTO предмета или null, если не найден</returns>
	public async Task<UchebniyPredmetDto> GetByIdAsync(long id)
	{
		var entity = await _context.UchebniyPredmet
			.FirstOrDefaultAsync(p => p.PredmetID == id);

		return entity == null ? null : entity.Adapt<UchebniyPredmetDto>();
	}

	/// <summary>
	/// Получает все учебные предметы, отсортированные по названию
	/// </summary>
	/// <returns>Коллекция DTO всех учебных предметов</returns>
	public async Task<IEnumerable<UchebniyPredmetDto>> GetAllAsync()
	{
		var entities = await _context.UchebniyPredmet
			.OrderBy(p => p.Nazvanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<UchebniyPredmetDto>>();
	}

	/// <summary>
	/// Обновляет существующий учебный предмет
	/// </summary>
	/// <param name="id">ID обновляемого предмета</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO предмета или null, если предмет не найден</returns>
	/// <exception cref="InvalidOperationException">Предмет с таким сокращением уже существует (дубликат)</exception>
	public async Task<UchebniyPredmetDto> UpdateAsync(long id, UchebniyPredmetDto dto)
	{
		var existing = await _context.UchebniyPredmet
			.FirstOrDefaultAsync(p => p.PredmetID == id);

		if(existing == null)
			return null;

		var isDuplicate = await IsDuplicateSokrashenieAsync(dto.Sokrashenie, id);
		if(isDuplicate)
			throw new InvalidOperationException($"Предмет с сокращением {dto.Sokrashenie} уже существует");

		dto.Adapt(existing);
		_context.UchebniyPredmet.Update(existing);
		await _context.SaveChangesAsync();

		return existing.Adapt<UchebniyPredmetDto>();
	}

	/// <summary>
	/// Удаляет учебный предмет по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого предмета</param>
	/// <returns>true - предмет удалён, false - предмет не найден</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.UchebniyPredmet
			.FirstOrDefaultAsync(p => p.PredmetID == id);

		if(entity == null)
			return false;

		_context.UchebniyPredmet.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование учебного предмета
	/// </summary>
	/// <param name="id">ID предмета</param>
	/// <returns>true - предмет существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.UchebniyPredmet
			.AnyAsync(p => p.PredmetID == id);
	}

	/// <summary>
	/// Проверяет наличие дубликата по сокращению (аббревиатуре) предмета
	/// </summary>
	/// <param name="sokrashenie">Сокращение предмета</param>
	/// <param name="excludeId">ID предмета для исключения из проверки (при обновлении)</param>
	/// <returns>true - дубликат существует, false - дубликатов нет</returns>
	public async Task<bool> IsDuplicateSokrashenieAsync(string sokrashenie, long? excludeId = null)
	{
		var query = _context.UchebniyPredmet
			.Where(p => p.Sokrashenie == sokrashenie);

		if(excludeId.HasValue)
		{
			query = query.Where(p => p.PredmetID != excludeId.Value);
		}

		return await query.AnyAsync();
	}
}

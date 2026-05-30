using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с учебными планами
/// </summary>
public sealed class UchebniyPlanRepository(SAPDbContext context, IMapper mapper) : IUchebniyPlanRepository
{
	private readonly SAPDbContext _context = context;
	private readonly IMapper _mapper = mapper;

	/// <summary>
	/// Создаёт новый учебный план
	/// </summary>
	/// <param name="dto">Данные для создания учебного плана</param>
	/// <returns>Созданный DTO учебного плана с заполненными полями</returns>
	public async Task<UchebniyPlanDto> CreateAsync(UchebniyPlanDto dto)
	{
		var entity = _mapper.Map<UchebniyPlanEntity>(dto);

		await _context.UchebniyPlan.AddAsync(entity);
		await _context.SaveChangesAsync();

		return _mapper.Map<UchebniyPlanDto>(entity);
	}

	/// <summary>
	/// Получает учебный план по идентификатору
	/// </summary>
	/// <param name="id">ID учебного плана</param>
	/// <returns>DTO учебного плана или null, если не найден</returns>
	public async Task<UchebniyPlanDto> GetByIdAsync(long id)
	{
		var entity = await _context.UchebniyPlan
			.FirstOrDefaultAsync(p => p.PlanID == id);

		return entity == null ? null : _mapper.Map<UchebniyPlanDto>(entity);
	}

	/// <summary>
	/// Получает все учебные планы, отсортированные по году начала и названию
	/// </summary>
	/// <returns>Коллекция DTO всех учебных планов</returns>
	public async Task<IEnumerable<UchebniyPlanDto>> GetAllAsync()
	{
		var entities = await _context.UchebniyPlan
			.OrderBy(p => p.GodNachala)
			.ThenBy(p => p.Nazvanie)
			.ToListAsync();

		return _mapper.Map<IEnumerable<UchebniyPlanDto>>(entities);
	}

	/// <summary>
	/// Получает учебные планы по году начала обучения
	/// </summary>
	/// <param name="godNachala">Год начала учебного плана</param>
	/// <returns>Коллекция DTO учебных планов за указанный год</returns>
	public async Task<IEnumerable<UchebniyPlanDto>> GetByGodNachalaAsync(int godNachala)
	{
		var entities = await _context.UchebniyPlan
			.Where(p => p.GodNachala == godNachala)
			.OrderBy(p => p.Nazvanie)
			.ToListAsync();

		return _mapper.Map<IEnumerable<UchebniyPlanDto>>(entities);
	}

	/// <summary>
	/// Выполняет поиск учебных планов по названию (содержит подстроку)
	/// </summary>
	/// <param name="searchTerm">Поисковый запрос (часть названия)</param>
	/// <returns>Коллекция DTO учебных планов, название которых содержит поисковый запрос</returns>
	/// <remarks>Если поисковый запрос пуст, возвращаются все учебные планы</remarks>
	public async Task<IEnumerable<UchebniyPlanDto>> SearchByNazvanieAsync(string searchTerm)
	{
		if(string.IsNullOrWhiteSpace(searchTerm))
			return await GetAllAsync();

		var entities = await _context.UchebniyPlan
			.Where(p => p.Nazvanie.Contains(searchTerm))
			.OrderBy(p => p.GodNachala)
			.ToListAsync();

		return _mapper.Map<IEnumerable<UchebniyPlanDto>>(entities);
	}

	/// <summary>
	/// Обновляет существующий учебный план
	/// </summary>
	/// <param name="id">ID обновляемого учебного плана</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO учебного плана или null, если план не найден</returns>
	public async Task<UchebniyPlanDto> UpdateAsync(long id, UchebniyPlanDto dto)
	{
		var existing = await _context.UchebniyPlan
			.FirstOrDefaultAsync(p => p.PlanID == id);

		if(existing == null)
			return null;

		_mapper.Map(dto, existing);

		_context.UchebniyPlan.Update(existing);
		await _context.SaveChangesAsync();

		return _mapper.Map<UchebniyPlanDto>(existing);
	}

	/// <summary>
	/// Удаляет учебный план по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого учебного плана</param>
	/// <returns>true - план удалён, false - план не найден</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.UchebniyPlan
			.FirstOrDefaultAsync(p => p.PlanID == id);

		if(entity == null)
			return false;

		_context.UchebniyPlan.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование учебного плана
	/// </summary>
	/// <param name="id">ID учебного плана</param>
	/// <returns>true - план существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.UchebniyPlan
			.AnyAsync(p => p.PlanID == id);
	}
}

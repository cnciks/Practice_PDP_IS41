using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с классами (группами учащихся)
/// </summary>
public sealed class KlassRepository : IKlassRepository
{
	private readonly SAPDbContext _context;
	private readonly IMapper _mapper;

	public KlassRepository(SAPDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	/// <summary>
	/// Создаёт новый класс
	/// </summary>
	/// <param name="dto">Данные для создания класса</param>
	/// <returns>Созданный DTO класса с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Класс на указанный учебный год уже существует
	/// - Сотрудник (руководитель) не найден
	/// - Учебный план не найден
	/// </exception>
	public async Task<KlassDto> CreateAsync(KlassDto dto)
	{
		var exists = await IsDuplicateKlassAsync(dto.NomerKlassa, dto.GodObucheniya);
		if(exists)
			throw new InvalidOperationException($"Класс {dto.NomerKlassa} на {dto.GodObucheniya} учебный год уже существует");

		var rukovoditel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.KlassRukovoditelID);
		if(rukovoditel == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.KlassRukovoditelID} не найден");

		var plan = await _context.UchebniyPlan
			.FirstOrDefaultAsync(p => p.PlanID == dto.PlanID);
		if(plan == null)
			throw new InvalidOperationException($"Учебный план с ID {dto.PlanID} не найден");

		var entity = _mapper.Map<KlassEntity>(dto);

		await _context.Klassy.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.KlassID);
	}

	/// <summary>
	/// Получает класс по идентификатору
	/// </summary>
	/// <param name="id">ID класса</param>
	/// <returns>DTO класса или null, если не найден</returns>
	public async Task<KlassDto> GetByIdAsync(long id)
	{
		var entity = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.FirstOrDefaultAsync(k => k.KlassID == id);

		if(entity == null)
			return null;

		var dto = _mapper.Map<KlassDto>(entity);

		if(entity.Sotrudnik != null)
		{
			dto.KlassRukovoditelFIO = $"{entity.Sotrudnik.Familia} {entity.Sotrudnik.Imya} {entity.Sotrudnik.Otchestvo}".Trim();
		}

		if(entity.UchebniyPlan != null)
		{
			dto.PlanNazvanie = entity.UchebniyPlan.Nazvanie;
		}

		return dto;
	}

	/// <summary>
	/// Получает все классы, отсортированные по году обучения и номеру класса
	/// </summary>
	/// <returns>Коллекция DTO всех классов</returns>
	public async Task<IEnumerable<KlassDto>> GetAllAsync()
	{
		var entities = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.OrderBy(k => k.GodObucheniya)
			.ThenBy(k => k.NomerKlassa)
			.ToListAsync();

		var dtos = _mapper.Map<IEnumerable<KlassDto>>(entities);

		foreach(var dto in dtos)
		{
			var entity = entities.First(e => e.KlassID == dto.KlassID);

			if(entity.Sotrudnik != null)
			{
				dto.KlassRukovoditelFIO = $"{entity.Sotrudnik.Familia} {entity.Sotrudnik.Imya} {entity.Sotrudnik.Otchestvo}".Trim();
			}

			if(entity.UchebniyPlan != null)
			{
				dto.PlanNazvanie = entity.UchebniyPlan.Nazvanie;
			}
		}

		return dtos;
	}

	/// <summary>
	/// Получает классы за указанный учебный год
	/// </summary>
	/// <param name="godObucheniya">Учебный год</param>
	/// <returns>Коллекция DTO классов за указанный год</returns>
	public async Task<IEnumerable<KlassDto>> GetByGodObucheniyaAsync(int godObucheniya)
	{
		var entities = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.Where(k => k.GodObucheniya == godObucheniya)
			.OrderBy(k => k.NomerKlassa)
			.ToListAsync();

		return await MapToDtoWithDetails(entities);
	}

	/// <summary>
	/// Получает классы по руководителю
	/// </summary>
	/// <param name="klassRukovoditelID">ID сотрудника-руководителя</param>
	/// <returns>Коллекция DTO классов, закреплённых за указанным руководителем</returns>
	public async Task<IEnumerable<KlassDto>> GetByKlassRukovoditelAsync(long klassRukovoditelID)
	{
		var entities = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.Where(k => k.KlassRukovoditelID == klassRukovoditelID)
			.OrderBy(k => k.GodObucheniya)
			.ThenBy(k => k.NomerKlassa)
			.ToListAsync();

		return await MapToDtoWithDetails(entities);
	}

	/// <summary>
	/// Получает классы по учебному плану
	/// </summary>
	/// <param name="planID">ID учебного плана</param>
	/// <returns>Коллекция DTO классов, использующих указанный учебный план</returns>
	public async Task<IEnumerable<KlassDto>> GetByPlanAsync(long planID)
	{
		var entities = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.Where(k => k.PlanID == planID)
			.OrderBy(k => k.GodObucheniya)
			.ThenBy(k => k.NomerKlassa)
			.ToListAsync();

		return await MapToDtoWithDetails(entities);
	}

	/// <summary>
	/// Обновляет существующий класс
	/// </summary>
	/// <param name="id">ID обновляемого класса</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO класса</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Класс на указанный учебный год уже существует (дубликат)
	/// - Сотрудник (руководитель) не найден
	/// - Учебный план не найден
	/// </exception>
	public async Task<KlassDto> UpdateAsync(long id, KlassDto dto)
	{
		var existing = await _context.Klassy
			.Include(k => k.Sotrudnik)
			.Include(k => k.UchebniyPlan)
			.FirstOrDefaultAsync(k => k.KlassID == id);

		if(existing == null)
			return null;

		var isDuplicate = await IsDuplicateKlassAsync(dto.NomerKlassa, dto.GodObucheniya, id);
		if(isDuplicate)
			throw new InvalidOperationException($"Класс {dto.NomerKlassa} на {dto.GodObucheniya} учебный год уже существует");

		var rukovoditel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.KlassRukovoditelID);
		if(rukovoditel == null)
			throw new InvalidOperationException($"Сотрудник с ID {dto.KlassRukovoditelID} не найден");

		var plan = await _context.UchebniyPlan
			.FirstOrDefaultAsync(p => p.PlanID == dto.PlanID);

		if(plan == null)
			throw new InvalidOperationException($"Учебный план с ID {dto.PlanID} не найден");

		_mapper.Map(dto, existing);

		_context.Klassy.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет класс по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого класса</param>
	/// <returns>true - класс удалён, false - класс не найден</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.Klassy
			.FirstOrDefaultAsync(k => k.KlassID == id);

		if(entity == null)
			return false;

		_context.Klassy.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование класса
	/// </summary>
	/// <param name="id">ID класса</param>
	/// <returns>true - класс существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.Klassy
			.AnyAsync(k => k.KlassID == id);
	}

	/// <summary>
	/// Проверяет наличие дубликата класса (одинаковый номер класса и учебный год)
	/// </summary>
	/// <param name="nomerKlassa">Номер класса</param>
	/// <param name="godObucheniya">Учебный год</param>
	/// <param name="excludeId">ID класса для исключения из проверки (при обновлении)</param>
	/// <returns>true - дубликат существует, false - дубликатов нет</returns>
	public async Task<bool> IsDuplicateKlassAsync(string nomerKlassa, int godObucheniya, long? excludeId = null)
	{
		var query = _context.Klassy
			.Where(k => k.NomerKlassa == nomerKlassa && k.GodObucheniya == godObucheniya);

		if(excludeId.HasValue)
		{
			query = query.Where(k => k.KlassID != excludeId.Value);
		}

		return await query.AnyAsync();
	}

	/// <summary>
	/// Вспомогательный метод для маппинга списка сущностей в DTO с заполнением дополнительных полей (ФИО руководителя, название плана)
	/// </summary>
	/// <param name="entities">Список сущностей классов</param>
	/// <returns>Коллекция DTO классов с заполненными деталями</returns>
	private async Task<IEnumerable<KlassDto>> MapToDtoWithDetails(List<KlassEntity> entities)
	{
		var dtos = _mapper.Map<IEnumerable<KlassDto>>(entities);

		foreach(var dto in dtos)
		{
			var entity = entities.First(e => e.KlassID == dto.KlassID);

			if(entity.Sotrudnik != null)
			{
				dto.KlassRukovoditelFIO = $"{entity.Sotrudnik.Familia} {entity.Sotrudnik.Imya} {entity.Sotrudnik.Otchestvo}".Trim();
			}

			if(entity.UchebniyPlan != null)
			{
				dto.PlanNazvanie = entity.UchebniyPlan.Nazvanie;
			}
		}

		return dtos;
	}
}

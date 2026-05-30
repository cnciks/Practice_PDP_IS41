using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с сотрудниками учебного заведения
/// </summary>
public sealed class SotrudnikRepository(SAPDbContext context, IMapper mapper) : ISotrudnikRepository
{
	private readonly SAPDbContext _context = context;
	private readonly IMapper _mapper = mapper;

	/// <summary>
	/// Создаёт нового сотрудника
	/// </summary>
	/// <param name="dto">Данные для создания сотрудника</param>
	/// <returns>Созданный DTO сотрудника с заполненными полями</returns>
	/// <remarks>Если статус не указан, по умолчанию устанавливается "Активен"</remarks>
	public async Task<SotrudnikDto> CreateAsync(SotrudnikDto dto)
	{
		var entity = _mapper.Map<SotrudnikEntity>(dto);

		if(string.IsNullOrEmpty(entity.Status))
			entity.Status = "Активен";

		await _context.Sotrudniki.AddAsync(entity);
		await _context.SaveChangesAsync();

		return _mapper.Map<SotrudnikDto>(entity);
	}

	/// <summary>
	/// Получает сотрудника по идентификатору
	/// </summary>
	/// <param name="id">ID сотрудника</param>
	/// <returns>DTO сотрудника или null, если не найден</returns>
	public async Task<SotrudnikDto> GetByIdAsync(long id)
	{
		var entity = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == id);

		return entity == null ? null : _mapper.Map<SotrudnikDto>(entity);
	}

	/// <summary>
	/// Получает всех сотрудников, отсортированных по фамилии и имени
	/// </summary>
	/// <returns>Коллекция DTO всех сотрудников</returns>
	public async Task<IEnumerable<SotrudnikDto>> GetAllAsync()
	{
		var entities = await _context.Sotrudniki
			.OrderBy(s => s.Familia)
			.ThenBy(s => s.Imya)
			.ToListAsync();

		return _mapper.Map<IEnumerable<SotrudnikDto>>(entities);
	}

	/// <summary>
	/// Получает сотрудников по должности
	/// </summary>
	/// <param name="dolzhnost">Должность сотрудника</param>
	/// <returns>Коллекция DTO сотрудников с указанной должностью</returns>
	public async Task<IEnumerable<SotrudnikDto>> GetByDolzhnostAsync(string dolzhnost)
	{
		var entities = await _context.Sotrudniki
			.Where(s => s.Dolzhnost == dolzhnost)
			.OrderBy(s => s.Familia)
			.ToListAsync();

		return _mapper.Map<IEnumerable<SotrudnikDto>>(entities);
	}

	/// <summary>
	/// Получает активных сотрудников (статус "Активен" или "Работает")
	/// </summary>
	/// <returns>Коллекция DTO активных сотрудников</returns>
	public async Task<IEnumerable<SotrudnikDto>> GetActiveAsync()
	{
		var entities = await _context.Sotrudniki
			.Where(s => s.Status == "Активен" || s.Status == "Работает")
			.OrderBy(s => s.Familia)
			.ToListAsync();

		return _mapper.Map<IEnumerable<SotrudnikDto>>(entities);
	}

	/// <summary>
	/// Обновляет существующего сотрудника
	/// </summary>
	/// <param name="id">ID обновляемого сотрудника</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO сотрудника или null, если сотрудник не найден</returns>
	public async Task<SotrudnikDto> UpdateAsync(long id, SotrudnikDto dto)
	{
		var existing = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == id);

		if(existing == null)
			return null;

		_mapper.Map(dto, existing);

		_context.Sotrudniki.Update(existing);
		await _context.SaveChangesAsync();

		return _mapper.Map<SotrudnikDto>(existing);
	}

	/// <summary>
	/// Удаляет сотрудника по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого сотрудника</param>
	/// <returns>true - сотрудник удалён, false - сотрудник не найден</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == id);

		if(entity == null)
			return false;

		_context.Sotrudniki.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование сотрудника
	/// </summary>
	/// <param name="id">ID сотрудника</param>
	/// <returns>true - сотрудник существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.Sotrudniki
			.AnyAsync(s => s.SotrudnikID == id);
	}
}

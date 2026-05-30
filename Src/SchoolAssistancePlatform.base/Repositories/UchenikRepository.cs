using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;
/// <summary>
/// Репозиторий для работы с учениками
/// </summary>
public sealed class UchenikRepository(SAPDbContext context) : IUchenikRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт нового ученика
	/// </summary>
	/// <param name="dto">Данные для создания ученика</param>
	/// <returns>Созданный DTO ученика с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">Класс с указанным ID не найден</exception>
	public async Task<UchenikDto> CreateAsync(UchenikDto dto)
	{
		var klass = await _context.Klassy
			.FirstOrDefaultAsync(k => k.KlassID == dto.KlassID);
		if(klass == null)
			throw new InvalidOperationException($"Класс с ID {dto.KlassID} не найден");

		var entity = dto.Adapt<UchenikEntity>();

		await _context.Uchenik.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.UchenikID);
	}

	/// <summary>
	/// Получает ученика по идентификатору
	/// </summary>
	/// <param name="id">ID ученика</param>
	/// <returns>DTO ученика или null, если не найден</returns>
	public async Task<UchenikDto> GetByIdAsync(long id)
	{
		var entity = await _context.Uchenik
			.Include(u => u.Klass)
			.FirstOrDefaultAsync(u => u.UchenikID == id);

		return entity == null ? null : entity.Adapt<UchenikDto>();
	}

	/// <summary>
	/// Получает всех учеников, отсортированных по фамилии и имени
	/// </summary>
	/// <returns>Коллекция DTO всех учеников</returns>
	public async Task<IEnumerable<UchenikDto>> GetAllAsync()
	{
		var entities = await _context.Uchenik
			.Include(u => u.Klass)
			.OrderBy(u => u.Familiia)
			.ThenBy(u => u.Imya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<UchenikDto>>();
	}

	/// <summary>
	/// Получает учеников по классу
	/// </summary>
	/// <param name="klassID">ID класса</param>
	/// <returns>Коллекция DTO учеников указанного класса</returns>
	public async Task<IEnumerable<UchenikDto>> GetByKlassAsync(long klassID)
	{
		var entities = await _context.Uchenik
			.Include(u => u.Klass)
			.Where(u => u.KlassID == klassID)
			.OrderBy(u => u.Familiia)
			.ThenBy(u => u.Imya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<UchenikDto>>();
	}

	/// <summary>
	/// Получает учеников по году обучения
	/// </summary>
	/// <param name="godObucheniya">Учебный год</param>
	/// <returns>Коллекция DTO учеников за указанный учебный год</returns>
	public async Task<IEnumerable<UchenikDto>> GetByGodObucheniyaAsync(int godObucheniya)
	{
		var entities = await _context.Uchenik
			.Include(u => u.Klass)
			.Where(u => u.Klass != null && u.Klass.GodObucheniya == godObucheniya)
			.OrderBy(u => u.Familiia)
			.ThenBy(u => u.Imya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<UchenikDto>>();
	}

	/// <summary>
	/// Выполняет поиск учеников по фамилии (содержит подстроку)
	/// </summary>
	/// <param name="familiia">Поисковый запрос (часть фамилии)</param>
	/// <returns>Коллекция DTO учеников, фамилия которых содержит поисковый запрос</returns>
	/// <remarks>Если поисковый запрос пуст, возвращаются все ученики</remarks>
	public async Task<IEnumerable<UchenikDto>> SearchByFamiliiaAsync(string familiia)
	{
		if(string.IsNullOrWhiteSpace(familiia))
			return await GetAllAsync();

		var entities = await _context.Uchenik
			.Include(u => u.Klass)
			.Where(u => u.Familiia.Contains(familiia))
			.OrderBy(u => u.Familiia)
			.ThenBy(u => u.Imya)
			.ToListAsync();

		return entities.Adapt<IEnumerable<UchenikDto>>();
	}

	/// <summary>
	/// Обновляет существующего ученика
	/// </summary>
	/// <param name="id">ID обновляемого ученика</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO ученика или null, если ученик не найден</returns>
	/// <exception cref="InvalidOperationException">Класс с указанным ID не найден</exception>
	public async Task<UchenikDto> UpdateAsync(long id, UchenikDto dto)
	{
		var existing = await _context.Uchenik
			.Include(u => u.Klass)
			.FirstOrDefaultAsync(u => u.UchenikID == id);

		if(existing == null)
			return null;

		var klass = await _context.Klassy
			.FirstOrDefaultAsync(k => k.KlassID == dto.KlassID);
		if(klass == null)
			throw new InvalidOperationException($"Класс с ID {dto.KlassID} не найден");

		dto.Adapt(existing);
		_context.Uchenik.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет ученика по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого ученика</param>
	/// <returns>true - ученик удалён, false - ученик не найден</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await _context.Uchenik
			.FirstOrDefaultAsync(u => u.UchenikID == id);

		if(entity == null)
			return false;

		_context.Uchenik.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование ученика
	/// </summary>
	/// <param name="id">ID ученика</param>
	/// <returns>true - ученик существует, false - не найден</returns>
	public async Task<bool> ExistsAsync(long id)
	{
		return await _context.Uchenik
			.AnyAsync(u => u.UchenikID == id);
	}
}


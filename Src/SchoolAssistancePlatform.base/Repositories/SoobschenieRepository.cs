using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с сообщениями (внутренняя переписка между сотрудниками)
/// </summary>
public sealed class SoobschenieRepository(SAPDbContext context) : ISoobschenieRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новое сообщение
	/// </summary>
	/// <param name="dto">Данные для создания сообщения</param>
	/// <returns>Созданный DTO сообщения с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Отправитель не найден
	/// - Получатель не найден
	/// </exception>
	public async Task<SoobschenieDto> CreateAsync(SoobschenieDto dto)
	{
		var otpravitel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.OtpravitelID);
		if(otpravitel == null)
			throw new InvalidOperationException($"Отправитель с ID {dto.OtpravitelID} не найден");

		var poluchatel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.PoluchatelID);
		if(poluchatel == null)
			throw new InvalidOperationException($"Получатель с ID {dto.PoluchatelID} не найден");

		var entity = dto.Adapt<SoobschenieEntity>();

		await _context.Soobschenie.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.SoobschenieID);
	}

	/// <summary>
	/// Получает сообщение по идентификатору
	/// </summary>
	/// <param name="id">ID сообщения</param>
	/// <returns>DTO сообщения или null, если не найдено</returns>
	public async Task<SoobschenieDto> GetByIdAsync(int id)
	{
		var entity = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.FirstOrDefaultAsync(s => s.SoobschenieID == id);

		return entity == null ? null : entity.Adapt<SoobschenieDto>();
	}

	/// <summary>
	/// Получает все сообщения, отсортированные по дате отправки (сначала новые)
	/// </summary>
	/// <returns>Коллекция DTO всех сообщений</returns>
	public async Task<IEnumerable<SoobschenieDto>> GetAllAsync()
	{
		var entities = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.OrderByDescending(s => s.DataOtpravki)
			.ToListAsync();

		return entities.Adapt<IEnumerable<SoobschenieDto>>();
	}

	/// <summary>
	/// Получает сообщения, отправленные указанным отправителем
	/// </summary>
	/// <param name="otpravitelID">ID отправителя</param>
	/// <returns>Коллекция DTO сообщений от указанного отправителя</returns>
	public async Task<IEnumerable<SoobschenieDto>> GetByOtpravitelAsync(long otpravitelID)
	{
		var entities = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.Where(s => s.OtpravitelID == otpravitelID)
			.OrderByDescending(s => s.DataOtpravki)
			.ToListAsync();

		return entities.Adapt<IEnumerable<SoobschenieDto>>();
	}

	/// <summary>
	/// Получает сообщения, полученные указанным получателем
	/// </summary>
	/// <param name="poluchatelID">ID получателя</param>
	/// <returns>Коллекция DTO сообщений для указанного получателя</returns>
	public async Task<IEnumerable<SoobschenieDto>> GetByPoluchatelAsync(long poluchatelID)
	{
		var entities = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.Where(s => s.PoluchatelID == poluchatelID)
			.OrderByDescending(s => s.DataOtpravki)
			.ToListAsync();

		return entities.Adapt<IEnumerable<SoobschenieDto>>();
	}

	/// <summary>
	/// Получает непрочитанные сообщения для указанного получателя
	/// </summary>
	/// <param name="poluchatelID">ID получателя</param>
	/// <returns>Коллекция DTO непрочитанных сообщений</returns>
	public async Task<IEnumerable<SoobschenieDto>> GetUnreadByPoluchatelAsync(long poluchatelID)
	{
		var entities = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.Where(s => s.PoluchatelID == poluchatelID && s.Prochitano == false)
			.OrderByDescending(s => s.DataOtpravki)
			.ToListAsync();

		return entities.Adapt<IEnumerable<SoobschenieDto>>();
	}

	/// <summary>
	/// Получает сообщения по типу получателя
	/// </summary>
	/// <param name="tipPoluchatelya">Тип получателя (ученик, учитель, родитель и т.д.)</param>
	/// <returns>Коллекция DTO сообщений для указанного типа получателя</returns>
	public async Task<IEnumerable<SoobschenieDto>> GetByTipPoluchatelyaAsync(string tipPoluchatelya)
	{
		var entities = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.Where(s => s.TipPoluchatelya == tipPoluchatelya)
			.OrderByDescending(s => s.DataOtpravki)
			.ToListAsync();

		return entities.Adapt<IEnumerable<SoobschenieDto>>();
	}

	/// <summary>
	/// Обновляет существующее сообщение
	/// </summary>
	/// <param name="id">ID обновляемого сообщения</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO сообщения</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Отправитель не найден
	/// - Получатель не найден
	/// </exception>
	public async Task<SoobschenieDto> UpdateAsync(int id, SoobschenieDto dto)
	{
		var existing = await _context.Soobschenie
			.Include(s => s.Otpravitel)
			.Include(s => s.Poluchatel)
			.FirstOrDefaultAsync(s => s.SoobschenieID == id);

		if(existing == null)
			return null;

		var otpravitel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.OtpravitelID);
		if(otpravitel == null)
			throw new InvalidOperationException($"Отправитель с ID {dto.OtpravitelID} не найден");

		var poluchatel = await _context.Sotrudniki
			.FirstOrDefaultAsync(s => s.SotrudnikID == dto.PoluchatelID);
		if(poluchatel == null)
			throw new InvalidOperationException($"Получатель с ID {dto.PoluchatelID} не найден");

		dto.Adapt(existing);
		_context.Soobschenie.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет сообщение по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого сообщения</param>
	/// <returns>true - сообщение удалено, false - сообщение не найдено</returns>
	public async Task<bool> DeleteAsync(int id)
	{
		var entity = await _context.Soobschenie
			.FirstOrDefaultAsync(s => s.SoobschenieID == id);

		if(entity == null)
			return false;

		_context.Soobschenie.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование сообщения
	/// </summary>
	/// <param name="id">ID сообщения</param>
	/// <returns>true - сообщение существует, false - не найдено</returns>
	public async Task<bool> ExistsAsync(int id)
	{
		return await _context.Soobschenie
			.AnyAsync(s => s.SoobschenieID == id);
	}

	/// <summary>
	/// Отмечает сообщение как прочитанное
	/// </summary>
	/// <param name="id">ID сообщения</param>
	public async Task MarkAsReadAsync(int id)
	{
		var entity = await _context.Soobschenie
			.FirstOrDefaultAsync(s => s.SoobschenieID == id);

		if(entity != null)
		{
			entity.Prochitano = true;
			await _context.SaveChangesAsync();
		}
	}

	/// <summary>
	/// Получает количество непрочитанных сообщений для указанного получателя
	/// </summary>
	/// <param name="poluchatelID">ID получателя</param>
	/// <returns>Количество непрочитанных сообщений</returns>
	public async Task<int> GetUnreadCountAsync(long poluchatelID)
	{
		return await _context.Soobschenie
			.CountAsync(s => s.PoluchatelID == poluchatelID && s.Prochitano == false);
	}
}

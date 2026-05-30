using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Repositories;

/// <summary>
/// Репозиторий для работы с материально-технической базой (имущество, оборудование, мебель)
/// </summary>
public sealed class MatTehBazaRepository(SAPDbContext context) : IMatTehBazaRepository
{
	private readonly SAPDbContext _context = context;

	/// <summary>
	/// Создаёт новую запись об имуществе
	/// </summary>
	/// <param name="dto">Данные для создания имущества</param>
	/// <returns>Созданный DTO имущества с заполненными полями</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Имущество с таким инвентарным номером уже существует
	/// - Приказ поступления не найден
	/// - Приказ списания не найден (если указан)
	/// </exception>
	public async Task<MatTehBazaDto> CreateAsync(MatTehBazaDto dto)
	{
		var isDuplicate = await IsDuplicateInvNomerAsync(dto.InvNomer);
		if(isDuplicate)
			throw new InvalidOperationException($"Имущество с инвентарным номером {dto.InvNomer} уже существует");

		var prikazPostupleniya = await _context.Prikazy
			.FirstOrDefaultAsync(p => p.PrikazID == dto.PrikazPostupleniyaID);
		if(prikazPostupleniya == null)
			throw new InvalidOperationException($"Приказ поступления с ID {dto.PrikazPostupleniyaID} не найден");

		if(dto.PrikazSpisaniyaID.HasValue)
		{
			var prikazSpisaniya = await _context.Prikazy
				.FirstOrDefaultAsync(p => p.PrikazID == dto.PrikazSpisaniyaID.Value);
			if(prikazSpisaniya == null)
				throw new InvalidOperationException($"Приказ списания с ID {dto.PrikazSpisaniyaID} не найден");
		}

		var entity = dto.Adapt<MatTehBazaEntity>();

		await _context.MatTehBaza.AddAsync(entity);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(entity.InventarID);
	}

	/// <summary>
	/// Получает запись об имуществе по идентификатору
	/// </summary>
	/// <param name="id">Инвентарный ID имущества</param>
	/// <returns>DTO имущества или null, если не найдено</returns>
	public async Task<MatTehBazaDto> GetByIdAsync(int id)
	{
		var entity = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.FirstOrDefaultAsync(m => m.InventarID == id);

		return entity == null ? null : entity.Adapt<MatTehBazaDto>();
	}

	/// <summary>
	/// Получает всё имущество, отсортированное по наименованию
	/// </summary>
	/// <returns>Коллекция DTO всех записей об имуществе</returns>
	public async Task<IEnumerable<MatTehBazaDto>> GetAllAsync()
	{
		var entities = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.OrderBy(m => m.Naimenovanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<MatTehBazaDto>>();
	}

	/// <summary>
	/// Получает имущество по типу
	/// </summary>
	/// <param name="tip">Тип имущества (оборудование, мебель и т.д.)</param>
	/// <returns>Коллекция DTO имущества указанного типа</returns>
	public async Task<IEnumerable<MatTehBazaDto>> GetByTipAsync(string tip)
	{
		var entities = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.Where(m => m.Tip == tip)
			.OrderBy(m => m.Naimenovanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<MatTehBazaDto>>();
	}

	/// <summary>
	/// Получает имущество по кабинету размещения
	/// </summary>
	/// <param name="kabinet">Номер кабинета</param>
	/// <returns>Коллекция DTO имущества, находящегося в указанном кабинете</returns>
	public async Task<IEnumerable<MatTehBazaDto>> GetByKabinetAsync(string kabinet)
	{
		var entities = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.Where(m => m.Kabinet == kabinet)
			.OrderBy(m => m.Naimenovanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<MatTehBazaDto>>();
	}

	/// <summary>
	/// Получает имущество по статусу
	/// </summary>
	/// <param name="status">Статус имущества (в наличии, списано и т.д.)</param>
	/// <returns>Коллекция DTO имущества с указанным статусом</returns>
	public async Task<IEnumerable<MatTehBazaDto>> GetByStatusAsync(string status)
	{
		var entities = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.Where(m => m.Status == status)
			.OrderBy(m => m.Naimenovanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<MatTehBazaDto>>();
	}

	/// <summary>
	/// Получает имущество по приказу поступления
	/// </summary>
	/// <param name="prikazPostupleniyaID">ID приказа поступления</param>
	/// <returns>Коллекция DTO имущества, оприходованного указанным приказом</returns>
	public async Task<IEnumerable<MatTehBazaDto>> GetByPrikazPostupleniyaAsync(int prikazPostupleniyaID)
	{
		var entities = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.Where(m => m.PrikazPostupleniyaID == prikazPostupleniyaID)
			.OrderBy(m => m.Naimenovanie)
			.ToListAsync();

		return entities.Adapt<IEnumerable<MatTehBazaDto>>();
	}

	/// <summary>
	/// Обновляет существующую запись об имуществе
	/// </summary>
	/// <param name="id">ID обновляемого имущества</param>
	/// <param name="dto">Новые данные</param>
	/// <returns>Обновлённый DTO имущества</returns>
	/// <exception cref="InvalidOperationException">
	/// Возникает если:
	/// - Имущество с таким инвентарным номером уже существует (дубликат)
	/// - Приказ поступления не найден
	/// - Приказ списания не найден (если указан)
	/// </exception>
	public async Task<MatTehBazaDto> UpdateAsync(int id, MatTehBazaDto dto)
	{
		var existing = await _context.MatTehBaza
			.Include(m => m.PrikazPostupleniya)
			.Include(m => m.PrikazSpisaniya)
			.FirstOrDefaultAsync(m => m.InventarID == id);

		if(existing == null)
			return null;

		var isDuplicate = await IsDuplicateInvNomerAsync(dto.InvNomer, id);
		if(isDuplicate)
			throw new InvalidOperationException($"Имущество с инвентарным номером {dto.InvNomer} уже существует");

		var prikazPostupleniya = await _context.Prikazy
			.FirstOrDefaultAsync(p => p.PrikazID == dto.PrikazPostupleniyaID);
		if(prikazPostupleniya == null)
			throw new InvalidOperationException($"Приказ поступления с ID {dto.PrikazPostupleniyaID} не найден");

		if(dto.PrikazSpisaniyaID.HasValue)
		{
			var prikazSpisaniya = await _context.Prikazy
				.FirstOrDefaultAsync(p => p.PrikazID == dto.PrikazSpisaniyaID.Value);
			if(prikazSpisaniya == null)
				throw new InvalidOperationException($"Приказ списания с ID {dto.PrikazSpisaniyaID} не найден");
		}

		dto.Adapt(existing);
		_context.MatTehBaza.Update(existing);
		await _context.SaveChangesAsync();

		return await GetByIdAsync(id);
	}

	/// <summary>
	/// Удаляет запись об имуществе по идентификатору
	/// </summary>
	/// <param name="id">ID удаляемого имущества</param>
	/// <returns>true - имущество удалено, false - имущество не найдено</returns>
	public async Task<bool> DeleteAsync(int id)
	{
		var entity = await _context.MatTehBaza
			.FirstOrDefaultAsync(m => m.InventarID == id);

		if(entity == null)
			return false;

		_context.MatTehBaza.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Проверяет существование записи об имуществе
	/// </summary>
	/// <param name="id">ID имущества</param>
	/// <returns>true - имущество существует, false - не найдено</returns>
	public async Task<bool> ExistsAsync(int id)
	{
		return await _context.MatTehBaza
			.AnyAsync(m => m.InventarID == id);
	}

	/// <summary>
	/// Проверяет наличие дубликата по инвентарному номеру
	/// </summary>
	/// <param name="invNomer">Инвентарный номер</param>
	/// <param name="excludeId">ID имущества для исключения из проверки (при обновлении)</param>
	/// <returns>true - дубликат существует, false - дубликатов нет</returns>
	public async Task<bool> IsDuplicateInvNomerAsync(string invNomer, int? excludeId = null)
	{
		var query = _context.MatTehBaza
			.Where(m => m.InvNomer == invNomer);

		if(excludeId.HasValue)
		{
			query = query.Where(m => m.InventarID != excludeId.Value);
		}

		return await query.AnyAsync();
	}
}

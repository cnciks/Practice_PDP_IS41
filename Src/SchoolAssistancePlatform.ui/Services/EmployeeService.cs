using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public class EmployeeService(
	ISotrudnikRepository sotrudnikRepository,
	IKlassRepository klassRepository,
	IRaspisanieRepository raspisanieRepository)
{
	private readonly ISotrudnikRepository _sotrudnikRepository   = sotrudnikRepository;
	private readonly IKlassRepository _klassRepository           = klassRepository;
	private readonly IRaspisanieRepository _raspisanieRepository = raspisanieRepository;

	public async Task<IEnumerable<SotrudnikDto>> GetAllEmployeesAsync()
	{
		return await _sotrudnikRepository.GetAllAsync();
	}

	public async Task<SotrudnikDto> GetEmployeeByIdAsync(long id)
	{
		return await _sotrudnikRepository.GetByIdAsync(id);
	}

	public async Task<SotrudnikDto> UpdateEmployeeAsync(long id, SotrudnikDto dto)
	{
		return await _sotrudnikRepository.UpdateAsync(id, dto);
	}

	public async Task<SotrudnikDto> CreateEmployeeAsync(SotrudnikDto dto)
	{
		return await _sotrudnikRepository.CreateAsync(dto);
	}

	public async Task<bool> DeleteEmployeeAsync(long id)
	{
		return await _sotrudnikRepository.DeleteAsync(id);
	}

	public async Task<string> GetClassLeadershipAsync(long sotrudnikID)
	{
		var klasses = await _klassRepository.GetByKlassRukovoditelAsync(sotrudnikID);
		var klass = klasses.FirstOrDefault();
		return klass?.NomerKlassa ?? "Не руководит";
	}

	public async Task<string> GetRoomNumberAsync(long sotrudnikID)
	{
		var raspisanie = await _raspisanieRepository.GetBySotrudnikAsync(sotrudnikID);
		var room = raspisanie.FirstOrDefault();
		return room?.Kabinet ?? "Не указан";
	}
}

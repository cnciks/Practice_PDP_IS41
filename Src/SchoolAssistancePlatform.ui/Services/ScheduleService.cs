using System.Collections.Generic;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class ScheduleService(
	IRaspisanieRepository     raspisanieRepository,
	IKlassRepository          klassRepository,
	ISotrudnikRepository      sotrudnikRepository,
	IUchebniyPredmetRepository predmetRepository)
{
	private readonly IRaspisanieRepository     _raspisanieRepository = raspisanieRepository;
	private readonly IKlassRepository          _klassRepository      = klassRepository;
	private readonly ISotrudnikRepository      _sotrudnikRepository  = sotrudnikRepository;
	private readonly IUchebniyPredmetRepository _predmetRepository   = predmetRepository;

	public async Task<IEnumerable<RaspisanieDto>> GetAllAsync()
		=> await _raspisanieRepository.GetAllAsync();

	public async Task<IEnumerable<RaspisanieDto>> GetByKlassAsync(long klassID)
		=> await _raspisanieRepository.GetByKlassAsync(klassID);

	public async Task<IEnumerable<RaspisanieDto>> GetBySotrudnikAsync(long sotrudnikID)
		=> await _raspisanieRepository.GetBySotrudnikAsync(sotrudnikID);

	public async Task<IEnumerable<KlassDto>> GetAllKlassesAsync()
		=> await _klassRepository.GetAllAsync();

	public async Task<IEnumerable<SotrudnikDto>> GetAllTeachersAsync()
		=> await _sotrudnikRepository.GetAllAsync();

	public async Task<IEnumerable<UchebniyPredmetDto>> GetAllSubjectsAsync()
		=> await _predmetRepository.GetAllAsync();

	public async Task<RaspisanieDto> CreateAsync(RaspisanieDto dto)
		=> await _raspisanieRepository.CreateAsync(dto);

	public async Task<RaspisanieDto> UpdateAsync(long id, RaspisanieDto dto)
		=> await _raspisanieRepository.UpdateAsync(id, dto);

	public async Task<bool> DeleteAsync(long id)
		=> await _raspisanieRepository.DeleteAsync(id);
}

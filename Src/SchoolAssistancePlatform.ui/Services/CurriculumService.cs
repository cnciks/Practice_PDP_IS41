using System.Collections.Generic;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class CurriculumService(
	IUchebniyPredmetRepository predmetRepository,
	IUchebniyPlanRepository    planRepository,
	IKlassRepository           klassRepository,
	IRaspisanieRepository      raspisanieRepository)
{
	private readonly IUchebniyPredmetRepository _predmetRepository     = predmetRepository;
	private readonly IUchebniyPlanRepository    _planRepository        = planRepository;
	private readonly IKlassRepository           _klassRepository       = klassRepository;
	private readonly IRaspisanieRepository      _raspisanieRepository  = raspisanieRepository;

	public async Task<IEnumerable<UchebniyPredmetDto>> GetAllSubjectsAsync()
		=> await _predmetRepository.GetAllAsync();

	public async Task<UchebniyPredmetDto> CreateSubjectAsync(UchebniyPredmetDto dto)
		=> await _predmetRepository.CreateAsync(dto);

	public async Task<UchebniyPredmetDto> UpdateSubjectAsync(long id, UchebniyPredmetDto dto)
		=> await _predmetRepository.UpdateAsync(id, dto);

	public async Task<bool> DeleteSubjectAsync(long id)
		=> await _predmetRepository.DeleteAsync(id);

	public async Task<IEnumerable<UchebniyPlanDto>> GetAllPlansAsync()
		=> await _planRepository.GetAllAsync();

	public async Task<UchebniyPlanDto> CreatePlanAsync(UchebniyPlanDto dto)
		=> await _planRepository.CreateAsync(dto);

	public async Task<UchebniyPlanDto> UpdatePlanAsync(long id, UchebniyPlanDto dto)
		=> await _planRepository.UpdateAsync(id, dto);

	public async Task<bool> DeletePlanAsync(long id)
		=> await _planRepository.DeleteAsync(id);

	public async Task<IEnumerable<KlassDto>> GetAllKlassesAsync()
		=> await _klassRepository.GetAllAsync();

	public async Task<IEnumerable<RaspisanieDto>> GetAllScheduleAsync()
		=> await _raspisanieRepository.GetAllAsync();
}

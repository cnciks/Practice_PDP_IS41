using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IUchebniyPlanRepository
{
	Task<UchebniyPlanDto> CreateAsync(UchebniyPlanDto dto);

	Task<UchebniyPlanDto> GetByIdAsync(long id);

	Task<IEnumerable<UchebniyPlanDto>> GetAllAsync();

	Task<IEnumerable<UchebniyPlanDto>> GetByGodNachalaAsync(int godNachala);

	Task<IEnumerable<UchebniyPlanDto>> SearchByNazvanieAsync(string searchTerm);

	Task<UchebniyPlanDto> UpdateAsync(long id, UchebniyPlanDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

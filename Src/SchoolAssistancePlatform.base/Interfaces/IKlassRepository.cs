using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IKlassRepository
{
	Task<KlassDto> CreateAsync(KlassDto dto);

	Task<KlassDto> GetByIdAsync(long id);

	Task<IEnumerable<KlassDto>> GetAllAsync();

	Task<IEnumerable<KlassDto>> GetByGodObucheniyaAsync(int godObucheniya);

	Task<IEnumerable<KlassDto>> GetByKlassRukovoditelAsync(long klassRukovoditelID);

	Task<IEnumerable<KlassDto>> GetByPlanAsync(long planID);

	Task<KlassDto> UpdateAsync(long id, KlassDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);

	Task<bool> IsDuplicateKlassAsync(string nomerKlassa, int godObucheniya, long? excludeId = null);
}

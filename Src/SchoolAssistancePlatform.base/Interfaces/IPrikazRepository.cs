using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IPrikazRepository
{
	Task<PrikazDto> CreateAsync(PrikazDto dto);

	Task<PrikazDto> GetByIdAsync(int id);

	Task<IEnumerable<PrikazDto>> GetAllAsync();

	Task<IEnumerable<PrikazDto>> GetByTipPrikazaAsync(string tipPrikaza);

	Task<IEnumerable<PrikazDto>> GetBySotrudnikAsync(long sotrudnikID);

	Task<IEnumerable<PrikazDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

	Task<PrikazDto> UpdateAsync(int id, PrikazDto dto);

	Task<bool> DeleteAsync(int id);

	Task<bool> ExistsAsync(int id);

	Task<bool> IsDuplicateNomerPrikazaAsync(string nomerPrikaza, int? excludeId = null);
}

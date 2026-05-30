using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface ISotrudnikRepository
{
	Task<SotrudnikDto> CreateAsync(SotrudnikDto dto);

	Task<SotrudnikDto> GetByIdAsync(long id);

	Task<IEnumerable<SotrudnikDto>> GetAllAsync();

	Task<IEnumerable<SotrudnikDto>> GetByDolzhnostAsync(string dolzhnost);

	Task<IEnumerable<SotrudnikDto>> GetActiveAsync();

	Task<SotrudnikDto> UpdateAsync(long id, SotrudnikDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

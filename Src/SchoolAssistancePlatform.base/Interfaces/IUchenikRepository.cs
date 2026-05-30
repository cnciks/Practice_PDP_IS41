using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IUchenikRepository
{
	Task<UchenikDto> CreateAsync(UchenikDto dto);

	Task<UchenikDto> GetByIdAsync(long id);

	Task<IEnumerable<UchenikDto>> GetAllAsync();

	Task<IEnumerable<UchenikDto>> GetByKlassAsync(long klassID);

	Task<IEnumerable<UchenikDto>> GetByGodObucheniyaAsync(int godObucheniya);

	Task<IEnumerable<UchenikDto>> SearchByFamiliiaAsync(string familiia);

	Task<UchenikDto> UpdateAsync(long id, UchenikDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

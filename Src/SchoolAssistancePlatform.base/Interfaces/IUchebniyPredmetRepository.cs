using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IUchebniyPredmetRepository
{
	Task<UchebniyPredmetDto> CreateAsync(UchebniyPredmetDto dto);

	Task<UchebniyPredmetDto> GetByIdAsync(long id);

	Task<IEnumerable<UchebniyPredmetDto>> GetAllAsync();

	Task<UchebniyPredmetDto> UpdateAsync(long id, UchebniyPredmetDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);

	Task<bool> IsDuplicateSokrashenieAsync(string sokrashenie, long? excludeId = null);
}

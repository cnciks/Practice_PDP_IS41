using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IFinansyRepository
{
	Task<FinansyDto> CreateAsync(FinansyDto dto);

	Task<FinansyDto> GetByIdAsync(long id);

	Task<IEnumerable<FinansyDto>> GetAllAsync();

	Task<IEnumerable<FinansyDto>> GetByUchenikAsync(long uchenikID);

	Task<IEnumerable<FinansyDto>> GetByTipOperaciiAsync(string tipOperacii);

	Task<IEnumerable<FinansyDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

	Task<decimal> GetTotalSumByUchenikAsync(long uchenikID);

	Task<decimal> GetBalanceByUchenikAsync(long uchenikID);

	Task<FinansyDto> UpdateAsync(long id, FinansyDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

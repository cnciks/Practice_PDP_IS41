using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IZhurnalUspevaemostiRepository
{
	Task<ZhurnalUspevaemostiDto> CreateAsync(ZhurnalUspevaemostiDto dto);

	Task<ZhurnalUspevaemostiDto> GetByIdAsync(long id);

	Task<IEnumerable<ZhurnalUspevaemostiDto>> GetAllAsync();

	Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByUchenikAsync(long uchenikID);

	Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByRaspisanieAsync(long raspisanieID);

	Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByKlassAsync(long klassID);

	Task<IEnumerable<ZhurnalUspevaemostiDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

	Task<double> GetAverageOcenkaByUchenikAsync(long uchenikID);

	Task<ZhurnalUspevaemostiDto> UpdateAsync(long id, ZhurnalUspevaemostiDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

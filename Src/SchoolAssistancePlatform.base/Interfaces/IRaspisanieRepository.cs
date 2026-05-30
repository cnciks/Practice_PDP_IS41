using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IRaspisanieRepository
{
	Task<RaspisanieDto> CreateAsync(RaspisanieDto dto);

	Task<RaspisanieDto> GetByIdAsync(long id);

	Task<IEnumerable<RaspisanieDto>> GetAllAsync();

	Task<IEnumerable<RaspisanieDto>> GetByKlassAsync(long klassID);

	Task<IEnumerable<RaspisanieDto>> GetBySotrudnikAsync(long sotrudnikID);

	Task<IEnumerable<RaspisanieDto>> GetByDenNedeliAsync(long denNedeli);

	Task<IEnumerable<RaspisanieDto>> GetByKlassAndDenAsync(long klassID, long denNedeli);

	Task<RaspisanieDto> UpdateAsync(long id, RaspisanieDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);

	Task<bool> IsDuplicateScheduleAsync(long klassID, long denNedeli, short nomerUroka, long? excludeId = null);
}

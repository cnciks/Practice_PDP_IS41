using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface ISoobschenieRepository
{
	Task<SoobschenieDto> CreateAsync(SoobschenieDto dto);

	Task<SoobschenieDto> GetByIdAsync(int id);

	Task<IEnumerable<SoobschenieDto>> GetAllAsync();

	Task<IEnumerable<SoobschenieDto>> GetByOtpravitelAsync(long otpravitelID);

	Task<IEnumerable<SoobschenieDto>> GetByPoluchatelAsync(long poluchatelID);

	Task<IEnumerable<SoobschenieDto>> GetUnreadByPoluchatelAsync(long poluchatelID);

	Task<IEnumerable<SoobschenieDto>> GetByTipPoluchatelyaAsync(string tipPoluchatelya);

	Task<SoobschenieDto> UpdateAsync(int id, SoobschenieDto dto);

	Task<bool> DeleteAsync(int id);

	Task<bool> ExistsAsync(int id);

	Task MarkAsReadAsync(int id);

	Task<int> GetUnreadCountAsync(long poluchatelID);
}

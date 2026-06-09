using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class MessagesService(
	ISoobschenieRepository soobschenieRepo,
	ISotrudnikRepository   sotrudnikRepo)
{
	private readonly ISoobschenieRepository _soobschenieRepo = soobschenieRepo;
	private readonly ISotrudnikRepository   _sotrudnikRepo   = sotrudnikRepo;

	public async Task<IEnumerable<SoobschenieDto>> GetInboxAsync(long poluchatelID)
		=> await _soobschenieRepo.GetByPoluchatelAsync(poluchatelID);

	public async Task<IEnumerable<SoobschenieDto>> GetOutboxAsync(long otpravitelID)
		=> await _soobschenieRepo.GetByOtpravitelAsync(otpravitelID);

	public async Task<IEnumerable<SoobschenieDto>> GetUnreadAsync(long poluchatelID)
		=> await _soobschenieRepo.GetUnreadByPoluchatelAsync(poluchatelID);

	public async Task<int> GetUnreadCountAsync(long poluchatelID)
		=> await _soobschenieRepo.GetUnreadCountAsync(poluchatelID);

	public async Task MarkAsReadAsync(int id)
		=> await _soobschenieRepo.MarkAsReadAsync(id);

	public async Task<SoobschenieDto> SendAsync(SoobschenieDto dto)
		=> await _soobschenieRepo.CreateAsync(dto);

	public async Task<bool> DeleteAsync(int id)
		=> await _soobschenieRepo.DeleteAsync(id);

	public async Task<IEnumerable<SotrudnikDto>> GetAllEmployeesAsync()
		=> await _sotrudnikRepo.GetAllAsync();

	public async Task<IEnumerable<string>> GetAvailableTypesAsync()
	{
		var all = await _soobschenieRepo.GetAllAsync();
		return all
			.Select(s => s.TipPoluchatelya)
			.Where(t => !string.IsNullOrWhiteSpace(t))
			.Distinct()
			.OrderBy(t => t)
			.ToList();
	}
}

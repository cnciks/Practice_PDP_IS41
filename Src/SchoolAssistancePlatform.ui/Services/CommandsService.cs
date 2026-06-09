using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class CommandsService(IPrikazRepository prikazRepository)
{
	private readonly IPrikazRepository _repo = prikazRepository;

	public async Task<IEnumerable<PrikazDto>> GetAllAsync()
		=> await _repo.GetAllAsync();

	public async Task<IEnumerable<PrikazDto>> GetByTipAsync(string tip)
		=> await _repo.GetByTipPrikazaAsync(tip);

	public async Task<IEnumerable<string>> GetAvailableTypesAsync()
	{
		var all = await _repo.GetAllAsync();
		return all
			.Select(p => p.TipPrikaza)
			.Where(t => !string.IsNullOrWhiteSpace(t))
			.Distinct()
			.OrderBy(t => t)
			.ToList();
	}

	public async Task<PrikazDto> CreateAsync(PrikazDto dto)
		=> await _repo.CreateAsync(dto);

	public async Task<PrikazDto> UpdateAsync(int id, PrikazDto dto)
		=> await _repo.UpdateAsync(id, dto);

	public async Task<bool> DeleteAsync(int id)
		=> await _repo.DeleteAsync(id);
}

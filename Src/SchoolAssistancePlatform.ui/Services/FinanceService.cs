using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class FinanceService(IFinansyRepository finansyRepository)
{
	private readonly IFinansyRepository _repo = finansyRepository;

	public async Task<IEnumerable<FinansyDto>> GetAllAsync()
		=> await _repo.GetAllAsync();

	public async Task<IEnumerable<FinansyDto>> GetByYearAsync(int year)
	{
		var start = new DateTime(year, 1, 1);
		var end   = new DateTime(year, 12, 31);
		return await _repo.GetByDateRangeAsync(start, end);
	}

	public async Task<IEnumerable<int>> GetAvailableYearsAsync()
	{
		var all = await _repo.GetAllAsync();
		return all
			.Select(f => f.DataPlatezha.Year)
			.Distinct()
			.OrderByDescending(y => y)
			.ToList();
	}

	public async Task<FinansyDto> CreateAsync(FinansyDto dto)
		=> await _repo.CreateAsync(dto);

	public async Task<FinansyDto> UpdateAsync(long id, FinansyDto dto)
		=> await _repo.UpdateAsync(id, dto);

	public async Task<bool> DeleteAsync(long id)
		=> await _repo.DeleteAsync(id);
}

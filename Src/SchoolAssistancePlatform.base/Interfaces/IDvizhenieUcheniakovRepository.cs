using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IDvizhenieUcheniakovRepository
{
	Task<DvizhenieUcheniakovDto> CreateAsync(DvizhenieUcheniakovDto dto);

	Task<DvizhenieUcheniakovDto> GetByIdAsync(long id);

	Task<IEnumerable<DvizhenieUcheniakovDto>> GetAllAsync();

	Task<IEnumerable<DvizhenieUcheniakovDto>> GetByUchenikAsync(long uchenikID);

	Task<IEnumerable<DvizhenieUcheniakovDto>> GetByTipDvizheniyaAsync(string tipDvizheniya);

	Task<IEnumerable<DvizhenieUcheniakovDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

	Task<DvizhenieUcheniakovDto> UpdateAsync(long id, DvizhenieUcheniakovDto dto);

	Task<bool> DeleteAsync(long id);

	Task<bool> ExistsAsync(long id);
}

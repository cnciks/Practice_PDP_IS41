using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base.Interfaces;

public interface IMatTehBazaRepository
{
	Task<MatTehBazaDto> CreateAsync(MatTehBazaDto dto);

	Task<MatTehBazaDto> GetByIdAsync(int id);

	Task<IEnumerable<MatTehBazaDto>> GetAllAsync();

	Task<IEnumerable<MatTehBazaDto>> GetByTipAsync(string tip);

	Task<IEnumerable<MatTehBazaDto>> GetByKabinetAsync(string kabinet);

	Task<IEnumerable<MatTehBazaDto>> GetByStatusAsync(string status);

	Task<IEnumerable<MatTehBazaDto>> GetByPrikazPostupleniyaAsync(int prikazPostupleniyaID);

	Task<MatTehBazaDto> UpdateAsync(int id, MatTehBazaDto dto);

	Task<bool> DeleteAsync(int id);

	Task<bool> ExistsAsync(int id);

	Task<bool> IsDuplicateInvNomerAsync(string invNomer, int? excludeId = null);
}

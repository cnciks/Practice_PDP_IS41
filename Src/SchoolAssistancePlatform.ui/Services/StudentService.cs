using System.Collections.Generic;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public class StudentService(
	IUchenikRepository uchenikRepository,
	IZhurnalUspevaemostiRepository zhurnalRepository)
{
	private readonly IUchenikRepository _uchenikRepository = uchenikRepository;
	private readonly IZhurnalUspevaemostiRepository _zhurnalRepository = zhurnalRepository;

	public async Task<IEnumerable<UchenikDto>> GetAllStudentsAsync()
	{
		return await _uchenikRepository.GetAllAsync();
	}

	public async Task<UchenikDto> GetStudentByIdAsync(long id)
	{
		return await _uchenikRepository.GetByIdAsync(id);
	}

	public async Task<UchenikDto> UpdateStudentAsync(long id, UchenikDto dto)
	{
		return await _uchenikRepository.UpdateAsync(id, dto);
	}

	public async Task<UchenikDto> CreateStudentAsync(UchenikDto dto)
	{
		return await _uchenikRepository.CreateAsync(dto);
	}

	public async Task<bool> DeleteStudentAsync(long id)
	{
		return await _uchenikRepository.DeleteAsync(id);
	}

	public async Task<double> GetAverageScoreAsync(long uchenikID)
	{
		return await _zhurnalRepository.GetAverageOcenkaByUchenikAsync(uchenikID);
	}

	public async Task<string> GetStudentFullNameAsync(long uchenikID)
	{
		var student = await _uchenikRepository.GetByIdAsync(uchenikID);
		if(student == null) return "Неизвестно";

		return string.IsNullOrEmpty(student.Otchestvo)
			? $"{student.Familiia} {student.Imya}"
			: $"{student.Familiia} {student.Imya} {student.Otchestvo}";
	}

	public async Task<string> GetStudentClassAsync(long uchenikID)
	{
		var student = await _uchenikRepository.GetByIdAsync(uchenikID);
		return student?.NomerKlassa ?? "Не указан";
	}

	public async Task<string> GetStudentParentsAsync(long uchenikID)
	{
		var student = await _uchenikRepository.GetByIdAsync(uchenikID);
		return student?.FIORoditeley ?? "Не указаны";
	}
}

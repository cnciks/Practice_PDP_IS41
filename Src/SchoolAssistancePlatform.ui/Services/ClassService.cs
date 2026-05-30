using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class ClassService(IKlassRepository klassRepository, IUchenikRepository uchenikRepository, ISotrudnikRepository sotrudnikRepository)
{
	private readonly IKlassRepository _klassRepository = klassRepository;
	private readonly IUchenikRepository _uchenikRepository = uchenikRepository;
	private readonly ISotrudnikRepository _sotrudnikRepository = sotrudnikRepository;

	public async Task<IEnumerable<KlassDto>> GetAllClassesAsync()
	{
		return await _klassRepository.GetAllAsync();
	}

	public async Task<KlassDto> GetClassByIdAsync(long id)
	{
		return await _klassRepository.GetByIdAsync(id);
	}

	public async Task<KlassDto> UpdateClassAsync(long id, KlassDto dto)
	{
		return await _klassRepository.UpdateAsync(id, dto);
	}

	public async Task<KlassDto> CreateClassAsync(KlassDto dto)
	{
		return await _klassRepository.CreateAsync(dto);
	}

	public async Task<bool> DeleteClassAsync(long id)
	{
		return await _klassRepository.DeleteAsync(id);
	}

	public async Task<int> GetStudentsCountAsync(long klassID)
	{
		var students = await _uchenikRepository.GetByKlassAsync(klassID);
		return students.Count();
	}

	public async Task<string> GetTeacherNameAsync(long klassRukovoditelID)
	{
		var teacher = await _sotrudnikRepository.GetByIdAsync(klassRukovoditelID);
		if(teacher == null) return "Не назначен";

		return string.IsNullOrEmpty(teacher.Otchestvo)
			? $"{teacher.Familia} {teacher.Imya}"
			: $"{teacher.Familia} {teacher.Imya} {teacher.Otchestvo}";
	}
}

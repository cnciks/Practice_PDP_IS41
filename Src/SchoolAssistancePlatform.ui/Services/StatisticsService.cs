using System;
using System.Linq;
using System.Threading.Tasks;

using SchoolAssistancePlatform.Base.Interfaces;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class StatisticsService(
	StudentService studentService,
	EmployeeService employeeService,
	ClassService classService,
	IZhurnalUspevaemostiRepository zhurnalRepository)
{
	private readonly StudentService _studentService = studentService;
	private readonly EmployeeService _employeeService = employeeService;
	private readonly ClassService _classService = classService;
	private readonly IZhurnalUspevaemostiRepository _zhurnal = zhurnalRepository;

	public async Task<int> GetStudentsCountAsync()
	{
		var all = await _studentService.GetAllStudentsAsync();
		return all.Count();
	}

	public async Task<int> GetEmployeesCountAsync()
	{
		var all = await _employeeService.GetAllEmployeesAsync();
		return all.Count();
	}

	public async Task<int> GetClassesCountAsync()
	{
		var all = await _classService.GetAllClassesAsync();
		return all.Count();
	}

	public async Task<double> GetSchoolAvgGradeAsync()
	{
		var all = await _zhurnal.GetAllAsync();
		var grades = all.Where(j => j.Ocenka > 0).Select(j => (double)j.Ocenka).ToList();
		return grades.Count > 0 ? grades.Average() : 0;
	}
}

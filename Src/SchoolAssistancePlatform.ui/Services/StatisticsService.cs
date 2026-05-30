using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class StatisticsService(
	StudentService studentService,
	EmployeeService employeeService,
	ClassService classService)
{
	private readonly StudentService _studentService = studentService;
	private readonly EmployeeService _employeeService = employeeService;
	private readonly ClassService _classService = classService;

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
}

using System;
using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal class EmployeeViewModel : ReactiveObject
{
	private readonly EmployeeService _employeeService;

	private SotrudnikDto _employee;
	private string _fullName;
	private string _position;
	private string _roomNumber;
	private string _classLeadership;
	private string _phone;

	public long Id => _employee?.SotrudnikID ?? 0;

	public string FullName
	{
		get => _fullName;
		set => this.RaiseAndSetIfChanged(ref _fullName, value);
	}

	public string Position
	{
		get => _position;
		set => this.RaiseAndSetIfChanged(ref _position, value);
	}

	public string RoomNumber
	{
		get => _roomNumber;
		set => this.RaiseAndSetIfChanged(ref _roomNumber, value);
	}

	public string ClassLeadership
	{
		get => _classLeadership;
		set => this.RaiseAndSetIfChanged(ref _classLeadership, value);
	}

	public string Phone
	{
		get => _phone;
		set => this.RaiseAndSetIfChanged(ref _phone, value);
	}

	public ReactiveCommand<Unit, Unit> EditCommand { get; }

	public EmployeeViewModel(
		SotrudnikDto employee,
		string classLeadership,
		string roomNumber,
		EmployeeService employeeService)
	{
		_employee        = employee;
		_employeeService = employeeService;
		_classLeadership = classLeadership;
		_roomNumber      = roomNumber;

		UpdateProperties();

		EditCommand = ReactiveCommand.CreateFromTask(Edit);
	}

	private void UpdateProperties()
	{
		FullName = string.IsNullOrEmpty(_employee.Otchestvo)
			? $"{_employee.Familia} {_employee.Imya}"
			: $"{_employee.Familia} {_employee.Imya} {_employee.Otchestvo}";

		Position = _employee.Dolzhnost ?? "Не указана";
		Phone = _employee.Telefon ?? "Не указан";
	}

	private async Task Edit()
	{
		try
		{
			var updateDto = new SotrudnikDto
			{
				Familia        = _employee.Familia,
				Imya           = _employee.Imya,
				Otchestvo      = _employee.Otchestvo,
				DataRozhdeniya = _employee.DataRozhdeniya,
				Dolzhnost      = _employee.Dolzhnost,
				Email          = _employee.Email,
				Telefon        = _employee.Telefon,
				DataPriema     = _employee.DataPriema,
				Status         = _employee.Status
			};
		}
		catch(Exception ex)
		{

		}
	}

	private async Task<bool> ShowEditDialog(SotrudnikDto dto)
	{
		var tcs = new TaskCompletionSource<bool>();

		return await tcs.Task;
	}
}

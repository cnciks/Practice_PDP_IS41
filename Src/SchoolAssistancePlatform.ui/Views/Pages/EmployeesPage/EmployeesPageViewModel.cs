using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal class EmployeesPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly EmployeeService _employeeService;

	private bool _applyFilter;

	#endregion

	#region Property

	public string Title => "Сотрудники";

	public Permissions Permission => Permissions.EmployeesPage;

	public ReactiveCommand<Unit, Task> AddEmployeesCommand { get; }

	public AvaloniaList<EmployeeViewModel> Employees { get; } = [];

	public bool ApplyFilter
	{
		get => _applyFilter;
		set => this.RaiseAndSetIfChanged(ref _applyFilter, value);
	}

	#endregion

	#region .ctor

	public EmployeesPageViewModel(EmployeeService employeeService)
	{
		_employeeService = employeeService;

		AddEmployeesCommand = ReactiveCommand.Create(AddEmployees);

		PropertyChanged += EmployeesPageViewModelPropertyChanged;
	}

	private void EmployeesPageViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(ApplyFilter):


				break;
		}
	}

	private async Task LoadEmployees()
	{
		try
		{
			var employees = await _employeeService.GetAllEmployeesAsync();

			var items = new List<EmployeeViewModel>();
			foreach(var emp in employees)
			{
				var classLeadership = await _employeeService.GetClassLeadershipAsync(emp.SotrudnikID);
				var roomNumber      = await _employeeService.GetRoomNumberAsync(emp.SotrudnikID);

				items.Add(new EmployeeViewModel(emp, classLeadership, roomNumber, _employeeService));
			}

			Employees.Clear();

			Employees.AddRange(items);
		}
		catch(Exception ex)
		{

		}
	}

	private Task AddEmployees()
	{
		return Task.CompletedTask;
	}

	public async Task LoadPageAsync()
	{
		await LoadEmployees();
	}

	#endregion
}

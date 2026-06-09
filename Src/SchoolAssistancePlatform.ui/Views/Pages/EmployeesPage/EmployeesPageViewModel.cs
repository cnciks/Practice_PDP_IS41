using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Collections;
using Avalonia.Media.Imaging;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;
using SchoolAssistancePlatform.UI.Views.WorkSpace;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal class EmployeesPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly EmployeeService _service;

	private List<EmployeeViewModel> _allEmployees = [];

	private string _searchText  = string.Empty;
	private bool   _applyFilter;
	private bool   _isFormVisible;
	private bool   _isEditMode;
	private long   _editingId;
	private string? _formError;

	#endregion

	#region Properties

	public string Title  => "Сотрудники";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/presentation.png");

	public Permissions Permission => Permissions.EmployeesPage;

	public AvaloniaList<EmployeeViewModel> Employees { get; } = [];

	public EmployeeFormViewModel Form { get; } = new();

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
	}

	public bool ApplyFilter
	{
		get => _applyFilter;
		set => this.RaiseAndSetIfChanged(ref _applyFilter, value);
	}

	public bool IsFormVisible
	{
		get => _isFormVisible;
		set => this.RaiseAndSetIfChanged(ref _isFormVisible, value);
	}

	public bool IsEditMode
	{
		get => _isEditMode;
		set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
	}

	public string FormTitle => _isEditMode ? "Редактировать сотрудника" : "Новый сотрудник";

	public string? FormError
	{
		get => _formError;
		set => this.RaiseAndSetIfChanged(ref _formError, value);
	}

	public ReactiveCommand<Unit, Unit> AddCommand { get; }

	public ReactiveCommand<Unit, Unit> CancelCommand { get; }

	public ReactiveCommand<Unit, Task> SaveCommand { get; }

	public ReactiveCommand<EmployeeViewModel, Unit> EditCommand { get; }

	public ReactiveCommand<EmployeeViewModel, Task> DeleteCommand { get; }

	#endregion

	#region .ctor

	public EmployeesPageViewModel(EmployeeService service)
	{
		_service = service;

		AddCommand    = ReactiveCommand.Create(OpenAdd);
		CancelCommand = ReactiveCommand.Create(CloseForm);
		SaveCommand   = ReactiveCommand.Create(Save);
		EditCommand   = ReactiveCommand.Create<EmployeeViewModel>(OpenEdit);
		DeleteCommand = ReactiveCommand.Create<EmployeeViewModel, Task>(Delete);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if(e.PropertyName == nameof(SearchText))
			ApplySearch();
	}

	private void ApplySearch()
	{
		var filtered = _allEmployees.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(e =>
				e.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				e.Dolzhnost.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				e.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				e.Telefon.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Employees.Clear();
		Employees.AddRange(filtered);
	}

	private void OpenAdd()
	{
		Form.Reset();
		FormError    = null;
		IsEditMode   = false;
		_editingId   = 0;
		IsFormVisible = true;
		this.RaisePropertyChanged(nameof(FormTitle));
	}

	private void OpenEdit(EmployeeViewModel vm)
	{
		Form.LoadFrom(vm);
		FormError    = null;
		IsEditMode   = true;
		_editingId   = vm.SotrudnikID;
		IsFormVisible = true;
		this.RaisePropertyChanged(nameof(FormTitle));
	}

	private void CloseForm()
	{
		IsFormVisible = false;
		FormError     = null;
	}

	private async Task Save()
	{
		FormError = null;

		if(!Form.IsValid)
		{
			FormError = "Заполните обязательные поля: Фамилия, Имя, Должность";
			return;
		}

		var dto = new SotrudnikDto
		{
			Familia        = Form.Familia,
			Imya           = Form.Imya,
			Otchestvo      = Form.Otchestvo,
			DataRozhdeniya = Form.DataRozhdeniya?.DateTime ?? DateTime.Today.AddYears(-30),
			Dolzhnost      = Form.Dolzhnost,
			Email          = Form.Email,
			Telefon        = Form.Telefon,
			DataPriema     = Form.DataPriema?.DateTime ?? DateTime.Today,
			Status         = Form.Status,
		};

		try
		{
			if(_isEditMode)
				await _service.UpdateEmployeeAsync(_editingId, dto);
			else
				await _service.CreateEmployeeAsync(dto);

			CloseForm();
			await LoadEmployees();
		}
		catch(Exception ex)
		{
			FormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task Delete(EmployeeViewModel vm)
	{
		try
		{
			await _service.DeleteEmployeeAsync(vm.SotrudnikID);
			_allEmployees.Remove(vm);
			ApplySearch();
		}
		catch(Exception) { }
	}

	private async Task LoadEmployees()
	{
		try
		{
			var dtos = await _service.GetAllEmployeesAsync();
			var items = new List<EmployeeViewModel>();

			foreach(var dto in dtos)
			{
				var cls  = await _service.GetClassLeadershipAsync(dto.SotrudnikID);
				var room = await _service.GetRoomNumberAsync(dto.SotrudnikID);
				items.Add(EmployeeViewModel.FromDto(dto, cls, room));
			}

			_allEmployees = items;
			ApplySearch();
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync() => await LoadEmployees();
}

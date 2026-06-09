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
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;
using SchoolAssistancePlatform.UI.Views.WorkSpace;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

internal class ClassesPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly ClassService    _classService;
	private readonly EmployeeService _employeeService;
	private readonly CurriculumService _curriculumService;

	private List<ClassItemViewModel> _allClasses = [];

	private string  _searchText   = string.Empty;
	private bool    _isFormVisible;
	private bool    _isEditMode;
	private long    _editingId;
	private string? _formError;

	#endregion

	#region Properties

	public string  Title => "Классы";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/blackboard.png");

	public Permissions Permission => Permissions.ClassesPage;

	public AvaloniaList<ClassItemViewModel> Classes  { get; } = [];

	public AvaloniaList<TeacherItem>        Teachers { get; } = [];

	public AvaloniaList<PlanItem>           Plans    { get; } = [];

	public ClassFormViewModel               Form     { get; } = new();

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
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

	public string FormTitle => _isEditMode ? "Редактировать класс" : "Новый класс";

	public string? FormError
	{
		get => _formError;
		set => this.RaiseAndSetIfChanged(ref _formError, value);
	}

	public ReactiveCommand<Unit, Unit>              AddCommand    { get; }
	public ReactiveCommand<Unit, Unit>              CancelCommand { get; }
	public ReactiveCommand<Unit, Task>              SaveCommand   { get; }
	public ReactiveCommand<ClassItemViewModel, Unit> EditCommand  { get; }
	public ReactiveCommand<ClassItemViewModel, Task> DeleteCommand { get; }

	#endregion

	#region .ctor

	public ClassesPageViewModel(
		ClassService      classService,
		EmployeeService   employeeService,
		CurriculumService curriculumService)
	{
		_classService      = classService;
		_employeeService   = employeeService;
		_curriculumService = curriculumService;

		AddCommand    = ReactiveCommand.Create(OpenAdd);
		CancelCommand = ReactiveCommand.Create(CloseForm);
		SaveCommand   = ReactiveCommand.Create(Save);
		EditCommand   = ReactiveCommand.Create<ClassItemViewModel>(OpenEdit);
		DeleteCommand = ReactiveCommand.Create<ClassItemViewModel, Task>(Delete);

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
		var filtered = _allClasses.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(c =>
				c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				c.Teacher.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Classes.Clear();
		Classes.AddRange(filtered);
	}

	private void OpenAdd()
	{
		Form.Reset(DateTime.Now.Year);
		if(Teachers.Count > 0) Form.SelectedTeacher = Teachers[0];
		if(Plans.Count > 0)    Form.SelectedPlan    = Plans[0];
		FormError     = null;
		IsEditMode    = false;
		_editingId    = 0;
		IsFormVisible = true;
		this.RaisePropertyChanged(nameof(FormTitle));
	}

	private void OpenEdit(ClassItemViewModel vm)
	{
		var teacher = Teachers.FirstOrDefault(t => t.SotrudnikID == vm.TeacherId);
		var plan    = Plans.FirstOrDefault(p => p.PlanID == vm.PlanID);
		Form.LoadFrom(vm, teacher, plan);
		FormError     = null;
		IsEditMode    = true;
		_editingId    = vm.KlassID;
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
			FormError = "Заполните обязательное поле: Номер класса";
			return;
		}

		try
		{
			var dto = Form.ToDto();

			if(_isEditMode)
				await _classService.UpdateClassAsync(_editingId, dto);
			else
				await _classService.CreateClassAsync(dto);

			CloseForm();
			await LoadClasses();
		}
		catch(Exception ex)
		{
			FormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task Delete(ClassItemViewModel vm)
	{
		try
		{
			await _classService.DeleteClassAsync(vm.KlassID);
			_allClasses.Remove(vm);
			ApplySearch();
		}
		catch(Exception) { }
	}

	private async Task LoadClasses()
	{
		try
		{
			var klasses = await _classService.GetAllClassesAsync();
			var items   = new List<ClassItemViewModel>();

			foreach(var k in klasses)
			{
				var count   = await _classService.GetStudentsCountAsync(k.KlassID);
				var teacher = await _classService.GetTeacherNameAsync(k.KlassRukovoditelID);
				items.Add(ClassItemViewModel.FromData(k, count, teacher));
			}

			_allClasses = items;
			ApplySearch();
		}
		catch(Exception) { }
	}

	private async Task LoadTeachers()
	{
		try
		{
			var employees = await _employeeService.GetAllEmployeesAsync();
			Teachers.Clear();
			Teachers.AddRange(employees.Select(TeacherItem.FromDto));
		}
		catch(Exception) { }
	}

	private async Task LoadPlans()
	{
		try
		{
			var plans = await _curriculumService.GetAllPlansAsync();
			Plans.Clear();
			Plans.AddRange(plans.Select(PlanItem.FromDto));
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadTeachers();
		await LoadPlans();
		await LoadClasses();
	}
}

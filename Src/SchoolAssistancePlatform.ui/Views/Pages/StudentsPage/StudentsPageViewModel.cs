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

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

internal class StudentsPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly StudentService _studentService;
	private readonly ClassService   _classService;

	private List<StudentViewModel> _allStudents = [];

	private string  _searchText   = string.Empty;
	private bool    _isFormVisible;
	private bool    _isEditMode;
	private long    _editingId;
	private string? _formError;

	#endregion

	#region Properties

	public string  Title      => "Ученики";

	public Bitmap? Icon       => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/students.png");

	public Permissions Permission => Permissions.StudentsPage;

	public AvaloniaList<StudentViewModel> Students { get; } = [];

	public AvaloniaList<KlassItem>        Klasses  { get; } = [];

	public StudentFormViewModel           Form     { get; } = new();

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

	public string FormTitle => _isEditMode ? "Редактировать ученика" : "Новый ученик";

	public string? FormError
	{
		get => _formError;
		set => this.RaiseAndSetIfChanged(ref _formError, value);
	}

	public ReactiveCommand<Unit, Unit>            AddCommand    { get; }
	public ReactiveCommand<Unit, Unit>            CancelCommand { get; }
	public ReactiveCommand<Unit, Task>            SaveCommand   { get; }
	public ReactiveCommand<StudentViewModel, Unit> EditCommand  { get; }
	public ReactiveCommand<StudentViewModel, Task> DeleteCommand { get; }

	#endregion

	#region .ctor

	public StudentsPageViewModel(StudentService studentService, ClassService classService)
	{
		_studentService = studentService;
		_classService   = classService;

		AddCommand    = ReactiveCommand.Create(OpenAdd);
		CancelCommand = ReactiveCommand.Create(CloseForm);
		SaveCommand   = ReactiveCommand.Create(Save);
		EditCommand   = ReactiveCommand.Create<StudentViewModel>(OpenEdit);
		DeleteCommand = ReactiveCommand.Create<StudentViewModel, Task>(Delete);

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
		var filtered = _allStudents.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(s =>
				s.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				s.Class.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				s.Parents.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Students.Clear();
		Students.AddRange(filtered);
	}

	private void OpenAdd()
	{
		Form.Reset();
		if(Klasses.Count > 0)
			Form.SelectedKlass = Klasses[0];
		FormError     = null;
		IsEditMode    = false;
		_editingId    = 0;
		IsFormVisible = true;
		this.RaisePropertyChanged(nameof(FormTitle));
	}

	private void OpenEdit(StudentViewModel vm)
	{
		var dto = new SchoolAssistancePlatform.framework.Data.UchenikDto
		{
			UchenikID         = vm.UchenikID,
			Familiia          = vm.Familiia,
			Imya              = vm.Imya,
			Otchestvo         = vm.Otchestvo,
			KlassID           = vm.KlassID,
			DataRozhdeniya    = DateTime.TryParse(vm.BirthDate, out var bd)
				? bd : DateTime.Today.AddYears(-12),
			FIORoditeley      = vm.Parents,
		};
		var klass = Klasses.FirstOrDefault(k => k.KlassID == vm.KlassID);
		Form.LoadFrom(dto, klass);
		FormError     = null;
		IsEditMode    = true;
		_editingId    = vm.UchenikID;
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
			FormError = "Заполните обязательные поля: Фамилия, Имя";
			return;
		}

		try
		{
			var dto = Form.ToDto();

			if(_isEditMode)
				await _studentService.UpdateStudentAsync(_editingId, dto);
			else
				await _studentService.CreateStudentAsync(dto);

			CloseForm();
			await LoadStudents();
		}
		catch(Exception ex)
		{
			FormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task Delete(StudentViewModel vm)
	{
		try
		{
			await _studentService.DeleteStudentAsync(vm.UchenikID);
			_allStudents.Remove(vm);
			ApplySearch();
		}
		catch(Exception) { }
	}

	private async Task LoadStudents()
	{
		try
		{
			var students = await _studentService.GetAllStudentsAsync();
			var items    = new List<StudentViewModel>();

			foreach(var s in students)
			{
				var avg      = await _studentService.GetAverageScoreAsync(s.UchenikID);
				var fullName = await _studentService.GetStudentFullNameAsync(s.UchenikID);
				var cls      = await _studentService.GetStudentClassAsync(s.UchenikID);
				var parents  = await _studentService.GetStudentParentsAsync(s.UchenikID);

				items.Add(StudentViewModel.FromDto(s, avg, fullName, cls, parents));
			}

			_allStudents = items;
			ApplySearch();
		}
		catch(Exception ex)
		{

		}
	}

	private async Task LoadKlasses()
	{
		try
		{
			var klasses = await _classService.GetAllClassesAsync();
			Klasses.Clear();
			Klasses.AddRange(klasses.Select(KlassItem.FromDto));
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadKlasses();
		await LoadStudents();
	}
}

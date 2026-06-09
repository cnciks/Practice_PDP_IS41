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

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal enum CurriculumTab { Matrix, Subjects, Plans }

internal class CurriculumPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly CurriculumService _service;

	private IReadOnlyList<KlassDto> _allKlasses = [];

	private KlassDto?        _filterKlass;
	private UchebniyPlanDto? _filterPlan;

	private CurriculumTab _activeTab = CurriculumTab.Matrix;

	// Subject form
	private bool    _isSubjectFormVisible;
	private bool    _isSubjectEditMode;
	private long    _editingSubjectId;
	private string? _subjectFormError;

	// Plan form
	private bool    _isPlanFormVisible;
	private bool    _isPlanEditMode;
	private long    _editingPlanId;
	private string? _planFormError;

	#endregion

	#region Properties

	public string  Title      => "Учебный план";
	public Bitmap? Icon       => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/folder.png");
	public Permissions Permission => Permissions.CurriculumPage;

	// Matrix
	public AvaloniaList<CurriculumFlatRowViewModel> Rows    { get; } = [];
	public AvaloniaList<string>                     Headers { get; } = [];
	public AvaloniaList<KlassDto>         FilterKlasses { get; } = [];
	public AvaloniaList<UchebniyPlanDto>  FilterPlans   { get; } = [];

	public KlassDto? FilterKlass
	{
		get => _filterKlass;
		set => this.RaiseAndSetIfChanged(ref _filterKlass, value);
	}

	public UchebniyPlanDto? FilterPlan
	{
		get => _filterPlan;
		set => this.RaiseAndSetIfChanged(ref _filterPlan, value);
	}

	// Tabs
	public CurriculumTab ActiveTab
	{
		get => _activeTab;
		set => this.RaiseAndSetIfChanged(ref _activeTab, value);
	}

	public bool IsMatrixTab   => _activeTab == CurriculumTab.Matrix;
	public bool IsSubjectsTab => _activeTab == CurriculumTab.Subjects;
	public bool IsPlansTab    => _activeTab == CurriculumTab.Plans;

	// Subjects CRUD
	public AvaloniaList<SubjectItemViewModel> Subjects { get; } = [];
	public SubjectFormViewModel SubjectForm { get; } = new();

	public bool IsSubjectFormVisible
	{
		get => _isSubjectFormVisible;
		set => this.RaiseAndSetIfChanged(ref _isSubjectFormVisible, value);
	}

	public bool IsSubjectEditMode
	{
		get => _isSubjectEditMode;
		set => this.RaiseAndSetIfChanged(ref _isSubjectEditMode, value);
	}

	public string SubjectFormTitle => _isSubjectEditMode ? "Редактировать предмет" : "Новый предмет";

	public string? SubjectFormError
	{
		get => _subjectFormError;
		set => this.RaiseAndSetIfChanged(ref _subjectFormError, value);
	}

	// Plans CRUD
	public AvaloniaList<PlanItemViewModel> Plans { get; } = [];
	public PlanFormViewModel PlanForm { get; } = new();

	public bool IsPlanFormVisible
	{
		get => _isPlanFormVisible;
		set => this.RaiseAndSetIfChanged(ref _isPlanFormVisible, value);
	}

	public bool IsPlanEditMode
	{
		get => _isPlanEditMode;
		set => this.RaiseAndSetIfChanged(ref _isPlanEditMode, value);
	}

	public string PlanFormTitle => _isPlanEditMode ? "Редактировать план" : "Новый план";

	public string? PlanFormError
	{
		get => _planFormError;
		set => this.RaiseAndSetIfChanged(ref _planFormError, value);
	}

	// Tab commands
	public ReactiveCommand<Unit, Unit> ShowMatrixCommand   { get; }
	public ReactiveCommand<Unit, Unit> ShowSubjectsCommand { get; }
	public ReactiveCommand<Unit, Unit> ShowPlansCommand    { get; }

	// Subject commands
	public ReactiveCommand<Unit, Unit>                AddSubjectCommand    { get; }
	public ReactiveCommand<Unit, Unit>                CancelSubjectCommand { get; }
	public ReactiveCommand<Unit, Task>                SaveSubjectCommand   { get; }
	public ReactiveCommand<SubjectItemViewModel, Unit> EditSubjectCommand  { get; }
	public ReactiveCommand<SubjectItemViewModel, Task> DeleteSubjectCommand { get; }

	// Plan commands
	public ReactiveCommand<Unit, Unit>             AddPlanCommand    { get; }
	public ReactiveCommand<Unit, Unit>             CancelPlanCommand { get; }
	public ReactiveCommand<Unit, Task>             SavePlanCommand   { get; }
	public ReactiveCommand<PlanItemViewModel, Unit> EditPlanCommand  { get; }
	public ReactiveCommand<PlanItemViewModel, Task> DeletePlanCommand { get; }

	#endregion

	#region .ctor

	public CurriculumPageViewModel(CurriculumService service)
	{
		_service = service;

		ShowMatrixCommand   = ReactiveCommand.Create(() => SwitchTab(CurriculumTab.Matrix));
		ShowSubjectsCommand = ReactiveCommand.Create(() => SwitchTab(CurriculumTab.Subjects));
		ShowPlansCommand    = ReactiveCommand.Create(() => SwitchTab(CurriculumTab.Plans));

		AddSubjectCommand    = ReactiveCommand.Create(OpenAddSubject);
		CancelSubjectCommand = ReactiveCommand.Create(CloseSubjectForm);
		SaveSubjectCommand   = ReactiveCommand.Create(SaveSubject);
		EditSubjectCommand   = ReactiveCommand.Create<SubjectItemViewModel>(OpenEditSubject);
		DeleteSubjectCommand = ReactiveCommand.Create<SubjectItemViewModel, Task>(DeleteSubject);

		AddPlanCommand    = ReactiveCommand.Create(OpenAddPlan);
		CancelPlanCommand = ReactiveCommand.Create(ClosePlanForm);
		SavePlanCommand   = ReactiveCommand.Create(SavePlan);
		EditPlanCommand   = ReactiveCommand.Create<PlanItemViewModel>(OpenEditPlan);
		DeletePlanCommand = ReactiveCommand.Create<PlanItemViewModel, Task>(DeletePlan);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void SwitchTab(CurriculumTab tab)
	{
		ActiveTab = tab;
		this.RaisePropertyChanged(nameof(IsMatrixTab));
		this.RaisePropertyChanged(nameof(IsSubjectsTab));
		this.RaisePropertyChanged(nameof(IsPlansTab));
		CloseSubjectForm();
		ClosePlanForm();
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(FilterKlass):
			case nameof(FilterPlan):
				_ = ReloadMatrixAsync();
				break;
		}
	}

	// ---- Subject form ----

	private void OpenAddSubject()
	{
		SubjectForm.Reset();
		SubjectFormError     = null;
		IsSubjectEditMode    = false;
		_editingSubjectId    = 0;
		IsSubjectFormVisible = true;
		this.RaisePropertyChanged(nameof(SubjectFormTitle));
	}

	private void OpenEditSubject(SubjectItemViewModel vm)
	{
		SubjectForm.LoadFrom(vm);
		SubjectFormError     = null;
		IsSubjectEditMode    = true;
		_editingSubjectId    = vm.PredmetID;
		IsSubjectFormVisible = true;
		this.RaisePropertyChanged(nameof(SubjectFormTitle));
	}

	private void CloseSubjectForm()
	{
		IsSubjectFormVisible = false;
		SubjectFormError     = null;
	}

	private async Task SaveSubject()
	{
		SubjectFormError = null;

		if(!SubjectForm.IsValid)
		{
			SubjectFormError = "Заполните обязательное поле: Название";
			return;
		}

		try
		{
			var dto = SubjectForm.ToDto();

			if(_isSubjectEditMode)
				await _service.UpdateSubjectAsync(_editingSubjectId, dto);
			else
				await _service.CreateSubjectAsync(dto);

			CloseSubjectForm();
			await LoadSubjects();
			await ReloadMatrixAsync();
		}
		catch(Exception ex)
		{
			SubjectFormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task DeleteSubject(SubjectItemViewModel vm)
	{
		try
		{
			await _service.DeleteSubjectAsync(vm.PredmetID);
			Subjects.Remove(vm);
			await ReloadMatrixAsync();
		}
		catch(Exception) { }
	}

	// ---- Plan form ----

	private void OpenAddPlan()
	{
		PlanForm.Reset();
		PlanFormError     = null;
		IsPlanEditMode    = false;
		_editingPlanId    = 0;
		IsPlanFormVisible = true;
		this.RaisePropertyChanged(nameof(PlanFormTitle));
	}

	private void OpenEditPlan(PlanItemViewModel vm)
	{
		PlanForm.LoadFrom(vm);
		PlanFormError     = null;
		IsPlanEditMode    = true;
		_editingPlanId    = vm.PlanID;
		IsPlanFormVisible = true;
		this.RaisePropertyChanged(nameof(PlanFormTitle));
	}

	private void ClosePlanForm()
	{
		IsPlanFormVisible = false;
		PlanFormError     = null;
	}

	private async Task SavePlan()
	{
		PlanFormError = null;

		if(!PlanForm.IsValid)
		{
			PlanFormError = "Заполните обязательное поле: Название";
			return;
		}

		try
		{
			var dto = PlanForm.ToDto();

			if(_isPlanEditMode)
				await _service.UpdatePlanAsync(_editingPlanId, dto);
			else
				await _service.CreatePlanAsync(dto);

			ClosePlanForm();
			await LoadPlans();
		}
		catch(Exception ex)
		{
			PlanFormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task DeletePlan(PlanItemViewModel vm)
	{
		try
		{
			await _service.DeletePlanAsync(vm.PlanID);
			Plans.Remove(vm);
		}
		catch(Exception) { }
	}

	// ---- Data loading ----

	private async Task LoadSubjects()
	{
		try
		{
			var subjects = await _service.GetAllSubjectsAsync();
			Subjects.Clear();
			Subjects.AddRange(subjects.Select(s => new SubjectItemViewModel
			{
				PredmetID  = s.PredmetID,
				Nazvanie   = s.Nazvanie   ?? string.Empty,
				Sokrashenie = s.Sokrashenie ?? string.Empty,
				ChasovNedelyu = s.ChasovNedelyu,
			}));
		}
		catch(Exception) { }
	}

	private async Task LoadPlans()
	{
		try
		{
			var plans = await _service.GetAllPlansAsync();
			var list  = plans.ToList();

			Plans.Clear();
			Plans.AddRange(list.Select(p => new PlanItemViewModel
			{
				PlanID     = p.PlanID,
				Nazvanie   = p.Nazvanie  ?? string.Empty,
				GodNachala = p.GodNachala,
				Opisanie   = p.Opisanie  ?? string.Empty,
			}));

			FilterPlans.Clear();
			FilterPlans.AddRange(list);
		}
		catch(Exception) { }
	}

	private async Task ReloadMatrixAsync()
	{
		try
		{
			var subjects = (await _service.GetAllSubjectsAsync()).ToList();
			var schedule = (await _service.GetAllScheduleAsync()).ToList();

			var visibleKlasses = FilterKlass is not null
				? _allKlasses.Where(k => k.KlassID == FilterKlass.KlassID).ToList()
				: FilterPlan is not null
					? _allKlasses.Where(k => k.PlanID == FilterPlan.PlanID).ToList()
					: _allKlasses.ToList();

			Headers.Clear();
			Headers.AddRange(visibleKlasses.Select(k => k.NomerKlassa));

			var hoursMap = schedule
				.GroupBy(r => (r.PredmetID, r.KlassID))
				.ToDictionary(g => g.Key, g => g.Count());

			var rows = new List<CurriculumFlatRowViewModel>();

			foreach(var subj in subjects.OrderBy(s => s.Nazvanie))
			{
				var cells = visibleKlasses.Select(klass =>
				{
					hoursMap.TryGetValue((subj.PredmetID, klass.KlassID), out var hours);
					var isOver = hours > subj.ChasovNedelyu;
					return new CurriculumCellViewModel
					{
						Text       = hours > 0 ? hours.ToString() : "—",
						HasLessons = hours > 0,
						IsOver     = isOver,
					};
				}).ToList();

				rows.Add(new CurriculumFlatRowViewModel
				{
					Subject   = subj.Nazvanie,
					Abbr      = subj.Sokrashenie,
					NormHours = subj.ChasovNedelyu,
					Cells     = cells,
				});
			}

			Rows.Clear();
			Rows.AddRange(rows);
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		try
		{
			var klasses = (await _service.GetAllKlassesAsync()).ToList();
			_allKlasses = klasses;
			FilterKlasses.Clear();
			FilterKlasses.AddRange(klasses);
		}
		catch(Exception) { }

		await LoadSubjects();
		await LoadPlans();
		await ReloadMatrixAsync();
	}
}

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

namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal class SchedulePageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private static readonly string[] LessonTimes =
	[
		"08:00–08:45",
		"08:55–09:40",
		"09:50–10:35",
		"10:55–11:40",
		"11:50–12:35",
		"12:45–13:30",
		"13:40–14:25",
	];

	private readonly ScheduleService _scheduleService;

	private IEnumerable<RaspisanieDto> _allItems = [];

	private KlassItem?    _filterKlass;
	private TeacherItem?  _filterTeacher;
	private bool          _applyFilter;
	private bool          _isFormVisible;
	private bool          _isEditMode;
	private long          _editingId;
	private string?       _formError;

	#endregion

	#region Properties

	public string  Title => "Расписание";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/schedule.png");

	public Permissions Permission => Permissions.SchedulePage;

	public AvaloniaList<ScheduleRowViewModel> ScheduleItems { get; } = [];

	public AvaloniaList<KlassItem>   FilterKlasses  { get; } = [];

	public AvaloniaList<TeacherItem> FilterTeachers { get; } = [];

	public AvaloniaList<KlassItem>   Klasses        { get; } = [];

	public AvaloniaList<SubjectItem> Subjects       { get; } = [];

	public AvaloniaList<TeacherItem> Teachers       { get; } = [];

	public ScheduleFormViewModel Form { get; } = new();

	public KlassItem? FilterKlass
	{
		get => _filterKlass;
		set => this.RaiseAndSetIfChanged(ref _filterKlass, value);
	}

	public TeacherItem? FilterTeacher
	{
		get => _filterTeacher;
		set => this.RaiseAndSetIfChanged(ref _filterTeacher, value);
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

	public string FormTitle => _isEditMode ? "Редактировать урок" : "Новый урок";

	public string? FormError
	{
		get => _formError;
		set => this.RaiseAndSetIfChanged(ref _formError, value);
	}

	public ReactiveCommand<Unit, Unit>              AddCommand          { get; }
	public ReactiveCommand<Unit, Unit>              CancelCommand       { get; }
	public ReactiveCommand<Unit, Task>              SaveCommand         { get; }
	public ReactiveCommand<LessonCellViewModel, Unit> EditLessonCommand { get; }
	public ReactiveCommand<LessonCellViewModel, Task> DeleteLessonCommand { get; }

	#endregion

	#region .ctor

	public SchedulePageViewModel(ScheduleService scheduleService)
	{
		_scheduleService = scheduleService;

		AddCommand          = ReactiveCommand.Create(OpenAdd);
		CancelCommand       = ReactiveCommand.Create(CloseForm);
		SaveCommand         = ReactiveCommand.Create(Save);
		EditLessonCommand   = ReactiveCommand.Create<LessonCellViewModel>(OpenEdit);
		DeleteLessonCommand = ReactiveCommand.Create<LessonCellViewModel, Task>(DeleteLesson);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(FilterKlass):
			case nameof(FilterTeacher):
			case nameof(ApplyFilter):
				RebuildRows(_allItems);
				break;
		}
	}

	private void OpenAdd()
	{
		Form.Reset();
		if(Klasses.Count > 0)   Form.SelectedKlass   = Klasses[0];
		if(Subjects.Count > 0)  Form.SelectedSubject = Subjects[0];
		if(Teachers.Count > 0)  Form.SelectedTeacher = Teachers[0];
		FormError     = null;
		IsEditMode    = false;
		_editingId    = 0;
		IsFormVisible = true;
		this.RaisePropertyChanged(nameof(FormTitle));
	}

	private void OpenEdit(LessonCellViewModel cell)
	{
		if(!cell.HasLesson) return;

		var klass   = Klasses.FirstOrDefault(k => k.KlassID == cell.KlassID);
		var subject = Subjects.FirstOrDefault(s => s.Nazvanie == cell.Subject);
		var teacher = Teachers.FirstOrDefault(t => t.FullName == cell.Teacher);

		Form.LoadFrom(cell, klass, subject, teacher);
		FormError     = null;
		IsEditMode    = true;
		_editingId    = cell.RaspisanieID ?? 0;
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
			FormError = "Заполните обязательные поля: Класс, Предмет, Учитель";
			return;
		}

		try
		{
			var dto = Form.ToDto();

			if(_isEditMode)
				await _scheduleService.UpdateAsync(_editingId, dto);
			else
				await _scheduleService.CreateAsync(dto);

			CloseForm();
			await ReloadSchedule();
		}
		catch(Exception ex)
		{
			FormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task DeleteLesson(LessonCellViewModel cell)
	{
		if(!cell.HasLesson || cell.RaspisanieID is null) return;

		try
		{
			await _scheduleService.DeleteAsync(cell.RaspisanieID.Value);
			await ReloadSchedule();
		}
		catch(Exception) { }
	}

	private async Task ReloadSchedule()
	{
		try
		{
			_allItems = await _scheduleService.GetAllAsync();
			RebuildRows(_allItems);
		}
		catch(Exception) { }
	}

	private void RebuildRows(IEnumerable<RaspisanieDto> source)
	{
		var filtered = source.AsEnumerable();

		if(ApplyFilter)
		{
			if(FilterKlass is not null)
				filtered = filtered.Where(r => r.KlassID == FilterKlass.KlassID);

			if(FilterTeacher is not null)
				filtered = filtered.Where(r => r.SotrudnikID == FilterTeacher.SotrudnikID);
		}

		var bySlot = filtered
			.GroupBy(r => r.NomerUroka)
			.ToDictionary(g => g.Key, g => g.ToList());

		var rows = new List<ScheduleRowViewModel>();
		for(var i = 1; i <= LessonTimes.Length; i++)
		{
			bySlot.TryGetValue((short)i, out var dayList);

			rows.Add(new ScheduleRowViewModel
			{
				Time      = LessonTimes[i - 1],
				Monday    = BuildCell(dayList, 1, i),
				Tuesday   = BuildCell(dayList, 2, i),
				Wednesday = BuildCell(dayList, 3, i),
				Thursday  = BuildCell(dayList, 4, i),
				Friday    = BuildCell(dayList, 5, i),
			});
		}

		ScheduleItems.Clear();
		ScheduleItems.AddRange(rows);
	}

	private static LessonCellViewModel BuildCell(
		List<RaspisanieDto>? dayList, int day, int nomerUroka)
	{
		var dto = dayList?.FirstOrDefault(r => r.DenNedeli == day);
		if(dto is null)
			return new LessonCellViewModel { DenNedeli = day, NomerUroka = (short)nomerUroka };

		return new LessonCellViewModel
		{
			RaspisanieID = dto.RaspisanieID,
			Subject      = dto.NazvaniePredmeta,
			Teacher      = dto.FIOPrepodaatelya,
			Room         = dto.Kabinet,
			DenNedeli    = dto.DenNedeli,
			NomerUroka   = dto.NomerUroka,
			KlassID      = dto.KlassID,
			SotrudnikID  = dto.SotrudnikID,
			PredmetID    = dto.PredmetID,
			NomerKlassa  = dto.NomerKlassa,
		};
	}

	#endregion

	public async Task LoadPageAsync()
	{
		try
		{
			var klasses  = await _scheduleService.GetAllKlassesAsync();
			var teachers = await _scheduleService.GetAllTeachersAsync();
			var subjects = await _scheduleService.GetAllSubjectsAsync();

			var klassItems   = klasses.Select(KlassItem.FromDto).ToList();
			var teacherItems = teachers.Select(TeacherItem.FromDto).ToList();
			var subjectItems = subjects.Select(SubjectItem.FromDto).ToList();

			FilterKlasses.Clear();
			FilterKlasses.AddRange(klassItems);

			FilterTeachers.Clear();
			FilterTeachers.AddRange(teacherItems);

			Klasses.Clear();
			Klasses.AddRange(klassItems);

			Teachers.Clear();
			Teachers.AddRange(teacherItems);

			Subjects.Clear();
			Subjects.AddRange(subjectItems);

			_allItems = await _scheduleService.GetAllAsync();
			RebuildRows(_allItems);
		}
		catch(Exception) { }
	}
}

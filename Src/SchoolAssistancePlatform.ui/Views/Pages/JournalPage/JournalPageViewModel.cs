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

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal class JournalPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private static readonly (string Label, int Months)[] PeriodOptions =
	[
		("Текущий месяц",       0),
		("Последние 3 месяца", -2),
		("Последние 6 месяцев",-5),
		("Весь год",          -11),
	];

	private readonly GradeJournalService _gradeJournalService;
	private readonly ClassService        _classService;

	private KlassDto?            _selectedKlass;
	private UchebniyPredmetItem? _selectedSubject;
	private PeriodItem?          _selectedPeriod;
	private bool                 _applyFilter;

	#endregion

	#region Properties

	public string Title => "Журнал";
	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/star.png");

	public Permissions Permission => Permissions.JournalPage;

	public AvaloniaList<StudentRowViewModel>  Students { get; } = [];
	public AvaloniaList<DateHeaderViewModel>  Dates    { get; } = [];

	public AvaloniaList<KlassDto>            Klasses  { get; } = [];
	public AvaloniaList<UchebniyPredmetItem> Subjects { get; } = [];
	public AvaloniaList<PeriodItem>          Periods  { get; } = [];

	public KlassDto? SelectedKlass
	{
		get => _selectedKlass;
		set => this.RaiseAndSetIfChanged(ref _selectedKlass, value);
	}

	public UchebniyPredmetItem? SelectedSubject
	{
		get => _selectedSubject;
		set => this.RaiseAndSetIfChanged(ref _selectedSubject, value);
	}

	public PeriodItem? SelectedPeriod
	{
		get => _selectedPeriod;
		set => this.RaiseAndSetIfChanged(ref _selectedPeriod, value);
	}

	public bool ApplyFilter
	{
		get => _applyFilter;
		set => this.RaiseAndSetIfChanged(ref _applyFilter, value);
	}

	public ReactiveCommand<Unit, Task> SaveJournalCommand { get; }

	#endregion

	#region .ctor

	public JournalPageViewModel(GradeJournalService gradeJournalService, ClassService classService)
	{
		_gradeJournalService = gradeJournalService;
		_classService        = classService;

		SaveJournalCommand = ReactiveCommand.Create(SaveJournal);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(SelectedKlass):
				_ = OnKlassChangedAsync();
				break;
			case nameof(SelectedSubject):
			case nameof(SelectedPeriod):
				_ = LoadJournal();
				break;
		}
	}

	private async Task OnKlassChangedAsync()
	{
		SelectedSubject = null;
		Subjects.Clear();

		if(SelectedKlass is not null)
		{
			var subjects = await _gradeJournalService.GetSubjectsByKlassAsync(SelectedKlass.KlassID);
			Subjects.AddRange(subjects.Select(UchebniyPredmetItem.FromDto));
		}

		await LoadJournal();
	}

	private async Task LoadFilters()
	{
		var klasses = await _classService.GetAllClassesAsync();
		Klasses.Clear();
		Klasses.AddRange(klasses);

		Periods.Clear();
		Periods.AddRange(PeriodOptions.Select(p => new PeriodItem(p.Label, p.Months)));
		SelectedPeriod = Periods.FirstOrDefault();
	}

	private async Task LoadJournal()
	{
		if(SelectedKlass is null)
		{
			Students.Clear();
			Dates.Clear();
			return;
		}

		try
		{
			var (start, end) = GetDateRange();
			var predmetID    = SelectedSubject?.PredmetID;

			var dates = await _gradeJournalService.GetLessonDatesAsync(
				SelectedKlass.KlassID, predmetID, start, end);

			Dates.Clear();
			Dates.AddRange(dates.Select(d => new DateHeaderViewModel { Date = d }));

			var students = await _gradeJournalService.GetStudentsWithGradesAsync(
				SelectedKlass.KlassID, predmetID, start, end);

			var rows = new List<StudentRowViewModel>();
			foreach(var s in students)
			{
				var cells = dates.Select(date =>
				{
					s.GradesByDate.TryGetValue(date, out var record);
					var cell = new GradeCellViewModel
					{
						StudentID    = s.StudentID,
						RaspisanieID = record?.RaspisanieID ?? 0,
						Date         = date,
						Value        = record?.Ocenka > 0 ? record.Ocenka.ToString() : string.Empty,
					};
					cell.MarkClean();
					return cell;
				}).ToList();

				rows.Add(new StudentRowViewModel
				{
					StudentID    = s.StudentID,
					StudentName  = s.StudentName,
					Grades       = cells,
					AverageGrade = s.AverageGrade,
				});
			}

			Students.Clear();
			Students.AddRange(rows);
		}
		catch(Exception ex)
		{
		}
	}

	private async Task SaveJournal()
	{
		try
		{
			var dirtyCells = Students
				.SelectMany(s => s.Grades)
				.Where(c => c.IsDirty)
				.ToList();

			foreach(var cell in dirtyCells)
			{
				if(cell.RaspisanieID == 0) continue;

				var ocenka = int.TryParse(cell.Value, out var v) ? v : 0;

				await _gradeJournalService.UpsertGradeAsync(
					cell.StudentID, cell.RaspisanieID, cell.Date, ocenka);

				cell.MarkClean();
			}

			foreach(var row in Students)
			{
				var valid = row.Grades
					.Where(c => int.TryParse(c.Value, out var g) && g > 0)
					.Select(c => int.Parse(c.Value))
					.ToList();

				row.AverageGrade = valid.Count > 0 ? valid.Average() : 0;
			}
		}
		catch(Exception)
		{
		}
	}

	private (DateTime start, DateTime end) GetDateRange()
	{
		var months = SelectedPeriod?.MonthsBack ?? 0;
		var now    = DateTime.Now;
		var start  = new DateTime(now.Year, now.Month, 1).AddMonths(months);
		return (start, now);
	}

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadFilters();
		await LoadJournal();
	}
}

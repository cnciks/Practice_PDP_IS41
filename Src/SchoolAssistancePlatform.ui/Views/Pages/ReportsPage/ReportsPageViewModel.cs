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

namespace SchoolAssistancePlatform.UI.Views.Pages.ReportsPage;

internal class ReportsPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly StatisticsService  _statistics;
	private readonly GradeJournalService _journal;
	private readonly FinanceService      _finance;
	private readonly ClassService        _classes;
	private readonly StudentService      _students;

	private string _activeTab    = "grades";
	private int    _selectedYear;
	private bool   _isLoading;

	#endregion

	#region Properties

	public string Title => "Отчёты";
	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/text-lines.png");
	public Permissions Permission => Permissions.ReportsPage;

	public AvaloniaList<ReportCardViewModel>       Cards       { get; } = [];
	public AvaloniaList<GradeReportRowViewModel>   GradeRows   { get; } = [];
	public AvaloniaList<FinanceReportRowViewModel> FinanceRows { get; } = [];
	public AvaloniaList<int>                       Years       { get; } = [];

	public string ActiveTab
	{
		get => _activeTab;
		set
		{
			this.RaiseAndSetIfChanged(ref _activeTab, value);
			this.RaisePropertyChanged(nameof(IsGradesTab));
			this.RaisePropertyChanged(nameof(IsFinanceTab));
		}
	}

	public int SelectedYear
	{
		get => _selectedYear;
		set => this.RaiseAndSetIfChanged(ref _selectedYear, value);
	}

	public bool IsLoading
	{
		get => _isLoading;
		set => this.RaiseAndSetIfChanged(ref _isLoading, value);
	}

	public bool IsGradesTab  => _activeTab == "grades";
	public bool IsFinanceTab => _activeTab == "finance";

	public ReactiveCommand<string, Task> SwitchTabCommand { get; }
	public ReactiveCommand<Unit, Task>   RefreshCommand   { get; }

	#endregion

	#region .ctor

	public ReportsPageViewModel(
		StatisticsService  statistics,
		GradeJournalService journal,
		FinanceService      finance,
		ClassService        classes,
		StudentService      students)
	{
		_statistics = statistics;
		_journal    = journal;
		_finance    = finance;
		_classes    = classes;
		_students   = students;

		_selectedYear = DateTime.Now.Year;

		SwitchTabCommand = ReactiveCommand.Create<string, Task>(SwitchTab);
		RefreshCommand   = ReactiveCommand.Create(Refresh);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if(e.PropertyName == nameof(SelectedYear))
			_ = LoadFinanceReportAsync();
	}

	private async Task SwitchTab(string tab)
	{
		ActiveTab = tab;
		await LoadActiveTabAsync();
	}

	private async Task Refresh() => await LoadActiveTabAsync();

	private async Task LoadActiveTabAsync()
	{
		switch(ActiveTab)
		{
			case "grades":  await LoadGradeReportAsync();   break;
			case "finance": await LoadFinanceReportAsync(); break;
		}
	}

	private async Task LoadSummaryCardsAsync()
	{
		try
		{
			var studentsCount  = await _statistics.GetStudentsCountAsync();
			var employeesCount = await _statistics.GetEmployeesCountAsync();
			var classesCount   = await _statistics.GetClassesCountAsync();

			var financeAll = await _finance.GetAllAsync();
			var currentYear = financeAll.Where(f => f.DataPlatezha.Year == DateTime.Now.Year).ToList();
			var income  = currentYear.Where(f => f.TipOperacii is "Приход" or "Оплата").Sum(f => f.Summa);
			var expense = currentYear.Where(f => f.TipOperacii is "Расход" or "Списание").Sum(f => f.Summa);

			Cards.Clear();
			Cards.AddRange(new[]
			{
				new ReportCardViewModel
				{
					Icon  = "👨‍🎓",
					Title = "Учеников",
					Value = studentsCount.ToString(),
					Sub   = "зачислено",
					Color = "#3498DB",
				},
				new ReportCardViewModel
				{
					Icon  = "👩‍🏫",
					Title = "Сотрудников",
					Value = employeesCount.ToString(),
					Sub   = "в штате",
					Color = "#9B59B6",
				},
				new ReportCardViewModel
				{
					Icon  = "🏫",
					Title = "Классов",
					Value = classesCount.ToString(),
					Sub   = "активных",
					Color = "#E67E22",
				},
				new ReportCardViewModel
				{
					Icon  = "💰",
					Title = "Доходы за год",
					Value = $"{income:N0} ₽",
					Sub   = $"расходы: {expense:N0} ₽",
					Color = "#27AE60",
				},
			});
		}
		catch(Exception) { }
	}

	private async Task LoadGradeReportAsync()
	{
		IsLoading = true;
		try
		{
			var klasses  = (await _classes.GetAllClassesAsync()).OrderBy(k => k.NomerKlassa).ToList();
			var allStudents = (await _students.GetAllStudentsAsync()).ToList();

			var rows = new List<GradeReportRowViewModel>();

			foreach(var klass in klasses)
			{
				var klassStudents = allStudents
					.Where(s => s.KlassID == klass.KlassID)
					.ToList();

				if(klassStudents.Count == 0) continue;

				var grades = new List<int>();
				foreach(var student in klassStudents)
				{
					var journalData = await _journal.GetStudentsWithGradesAsync(
						klass.KlassID, null, DateTime.Now.AddYears(-1), DateTime.Now);

					var studentData = journalData.FirstOrDefault(s => s.StudentID == student.UchenikID);
					if(studentData is not null)
					{
						foreach(var entry in studentData.GradesByDate.Values)
							if(entry.Ocenka > 0)
								grades.Add(entry.Ocenka);
					}
				}

				var avg = grades.Count > 0 ? grades.Average() : 0;

				rows.Add(new GradeReportRowViewModel
				{
					Klass        = klass.NomerKlassa,
					Students     = klassStudents.Count,
					AvgGrade     = avg,
					Excellent    = grades.Count(g => g == 5),
					Good         = grades.Count(g => g == 4),
					Satisfactory = grades.Count(g => g == 3),
					Poor         = grades.Count(g => g == 2),
				});
			}

			GradeRows.Clear();
			GradeRows.AddRange(rows);
		}
		catch(Exception) { }
		finally { IsLoading = false; }
	}

	private async Task LoadFinanceReportAsync()
	{
		IsLoading = true;
		try
		{
			var all = (await _finance.GetByYearAsync(SelectedYear)).ToList();

			var months = Enumerable.Range(1, 12).Select(m =>
			{
				var monthData = all.Where(f => f.DataPlatezha.Month == m).ToList();
				var income  = monthData.Where(f => f.TipOperacii is "Приход" or "Оплата").Sum(f => f.Summa);
				var expense = monthData.Where(f => f.TipOperacii is "Расход" or "Списание").Sum(f => f.Summa);

				return new FinanceReportRowViewModel
				{
					Month   = new DateTime(SelectedYear, m, 1).ToString("MMMM"),
					Income  = income,
					Expense = expense,
				};
			}).ToList();

			FinanceRows.Clear();
			FinanceRows.AddRange(months);
		}
		catch(Exception) { }
		finally { IsLoading = false; }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		var years = (await _finance.GetAvailableYearsAsync()).ToList();
		if(!years.Contains(DateTime.Now.Year))
			years.Insert(0, DateTime.Now.Year);

		Years.Clear();
		Years.AddRange(years);

		await LoadSummaryCardsAsync();
		await LoadGradeReportAsync();
	}
}

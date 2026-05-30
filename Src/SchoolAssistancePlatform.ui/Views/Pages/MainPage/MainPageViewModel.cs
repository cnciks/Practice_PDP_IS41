using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Timers;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.MainPage;

internal class MainPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly MenuActions _menuActions;
	private readonly StatisticsService _statisticsService;
	private string _greeting    = "Загрузка...";
	private string _currentDate = "Загрузка...";

	private Timer _timer;

	private int _studentsCount;
	private int _employeesCount;
	private int _classesCount;

	#endregion

	#region Property

	public string Title => "Главная страница";

	public Permissions Permission => Permissions.MainPage;

	public string Greeting
	{
		get => _greeting;
		set => this.RaiseAndSetIfChanged(ref _greeting, value);
	}

	public string CurrentDate
	{
		get => _currentDate;
		set => this.RaiseAndSetIfChanged(ref _currentDate, value);
	}

	public int StudentsCount
	{
		get => _studentsCount;
		set => this.RaiseAndSetIfChanged(ref _studentsCount, value);
	}

	public int EmployeesCount
	{
		get => _employeesCount;
		set => this.RaiseAndSetIfChanged(ref _employeesCount, value);
	}

	public int ClassesCount
	{
		get => _classesCount;
		set => this.RaiseAndSetIfChanged(ref _classesCount, value);
	}

	public AvaloniaList<PrikazDto> Orders { get; } = [];

	public ReactiveCommand<string, Unit> OpenMenuByNameCommand { get; }

	#endregion

	public MainPageViewModel(
		MenuActions menuActions,
		StatisticsService statisticsService)
	{
		_menuActions       = menuActions;
		_statisticsService = statisticsService;

		OpenMenuByNameCommand = ReactiveCommand.Create<string>(OpenMenuByName);

		UpdateGreeting();
		_timer = new Timer(60000);
		_timer.Elapsed += (s, e) => UpdateGreeting();
		_timer.Start();
	}

	private void OpenMenuByName(string name)
	{
		_menuActions.SelectMenuByName(name);
	}

	private void UpdateGreeting()
	{
		var hour = DateTime.Now.Hour;

		string greeting = hour switch
		{
			 >= 5 and < 12 => "Доброе утро",
			 >= 12 and < 18 => "Добрый день",
			 >= 18 and < 23 => "Добрый вечер",
			_ => "Доброй ночи"
		};

		Greeting    = greeting;
		CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
	}

	public async Task LoadPageAsync()
	{
		await LoadStatistics();
	}

	private async Task LoadStatistics()
	{
		try
		{
			StudentsCount  = await _statisticsService.GetStudentsCountAsync();
			EmployeesCount = await _statisticsService.GetEmployeesCountAsync();
			ClassesCount   = await _statisticsService.GetClassesCountAsync();
		}
		catch(Exception ex)
		{

		}
	}
}

using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Timers;

using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;
using SchoolAssistancePlatform.UI.Views.WorkSpace;

namespace SchoolAssistancePlatform.UI.Views.Pages.MainPage;

internal class MainPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly MenuActions       _menuActions;
	private readonly StatisticsService _statistics;
	private readonly CommandsService   _commands;
	private readonly MessagesService   _messages;
	private readonly AccountService    _account;

	private Timer _timer;

	private string _greeting    = string.Empty;
	private string _currentDate = string.Empty;
	private int    _studentsCount;
	private int    _employeesCount;
	private int    _classesCount;
	private int    _inboxCount;
	private int    _unreadCount;
	private double _avgGrade;

	#endregion

	#region Properties

	public string Title => "Главная страница";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/globe.png");

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

	public int InboxCount
	{
		get => _inboxCount;
		set => this.RaiseAndSetIfChanged(ref _inboxCount, value);
	}

	public int UnreadCount
	{
		get => _unreadCount;
		set => this.RaiseAndSetIfChanged(ref _unreadCount, value);
	}

	public double AvgGrade
	{
		get => _avgGrade;
		set => this.RaiseAndSetIfChanged(ref _avgGrade, value);
	}

	public string AvgGradeText  => _avgGrade > 0 ? _avgGrade.ToString("F2") : "—";
	public string AvgGradeColor => _avgGrade switch
	{
		>= 4.5 => "#27AE60",
		>= 3.5 => "#3498DB",
		>= 2.5 => "#E67E22",
		> 0    => "#E74C3C",
		_      => "#AAAAAA",
	};

	public AvaloniaList<RecentPrikazViewModel> RecentOrders { get; } = [];

	public ReactiveCommand<string, Unit> OpenMenuByNameCommand { get; }

	#endregion

	public MainPageViewModel(
		MenuActions        menuActions,
		StatisticsService  statistics,
		CommandsService    commands,
		MessagesService    messages,
		AccountService     account)
	{
		_menuActions = menuActions;
		_statistics  = statistics;
		_commands    = commands;
		_messages    = messages;
		_account     = account;

		OpenMenuByNameCommand = ReactiveCommand.Create<string>(name => _menuActions.SelectMenuByName(name));

		UpdateGreeting();
		_timer = new Timer(60_000);
		_timer.Elapsed += (_, _) => Dispatcher.UIThread.Post(UpdateGreeting);
		_timer.Start();
	}

	private void UpdateGreeting()
	{
		var hour = DateTime.Now.Hour;
		var salutation = hour switch
		{
			>= 5  and < 12 => "Доброе утро",
			>= 12 and < 18 => "Добрый день",
			>= 18 and < 23 => "Добрый вечер",
			_              => "Доброй ночи",
		};

		var name = _account.AccountUser?.SotrudnikFIO;
		Greeting = string.IsNullOrWhiteSpace(name)
			? salutation
			: $"{salutation}, {name}";

		CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
	}

	public async Task LoadPageAsync()
	{
		UpdateGreeting();

		await LoadCountersAsync();
		await LoadMessagesAsync();
		await LoadOrdersAsync();
		await LoadAvgGradeAsync();
	}

	private async Task LoadCountersAsync()
	{
		try
		{
			StudentsCount  = await _statistics.GetStudentsCountAsync();
			EmployeesCount = await _statistics.GetEmployeesCountAsync();
			ClassesCount   = await _statistics.GetClassesCountAsync();
		}
		catch(Exception) { }
	}

	private async Task LoadMessagesAsync()
	{
		try
		{
			var userId = _account.AccountUser?.Id ?? 0;
			if(userId == 0) return;

			var inbox  = await _messages.GetInboxAsync(userId);
			var unread = await _messages.GetUnreadCountAsync(userId);

			InboxCount  = inbox.Count();
			UnreadCount = unread;
		}
		catch(Exception) { }
	}

	private async Task LoadOrdersAsync()
	{
		try
		{
			var all = await _commands.GetAllAsync();
			var recent = all
				.OrderByDescending(p => p.DataPrikaza)
				.Take(5)
				.Select(p => new RecentPrikazViewModel
				{
					Nomer       = p.NomerPrikaza,
					Tip         = p.TipPrikaza,
					Soderzhanie = p.Soderzhanie,
					Date        = p.DataPrikaza,
				});

			RecentOrders.Clear();
			RecentOrders.AddRange(recent);
		}
		catch(Exception) { }
	}

	private async Task LoadAvgGradeAsync()
	{
		try
		{
			AvgGrade = await _statistics.GetSchoolAvgGradeAsync();
			this.RaisePropertyChanged(nameof(AvgGradeText));
			this.RaisePropertyChanged(nameof(AvgGradeColor));
		}
		catch(Exception) { }
	}
}

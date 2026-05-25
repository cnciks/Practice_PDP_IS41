using System;
using System.Reactive;
using System.Timers;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.MainPage;

internal class MainPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly MenuActions _menuActions;

	private string _greeting    = "Загрузка...";
	private string _currentDate = "Загрузка...";

	private Timer _timer;

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

	public AvaloniaList<PrikazDto> Orders { get; } = [];

	public ReactiveCommand<string, Unit> OpenMenuByNameCommand { get; }

	#endregion

	public MainPageViewModel(MenuActions menuActions)
	{
		_menuActions = menuActions;

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
}

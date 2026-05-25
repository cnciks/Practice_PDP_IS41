using System.Collections.Generic;
using System.Linq;
using System.Reactive;

using Autofac;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Styling;

using ReactiveUI;

using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.WorkSpace;

internal class WorkSpaceViewModel : ReactiveObject
{
	#region Data

	private MenuItem _selectedMenuItem;

	private readonly AccountService _accountService;

	private IReadOnlyList<MenuItem> _menuItems;
	private string _workSpaceOwnerName;

	#endregion

	#region Property

	public string WorkSpaceOwnerName
	{
		get => _workSpaceOwnerName;
		set => this.RaiseAndSetIfChanged(ref _workSpaceOwnerName, value);
	}

	internal AvaloniaList<MenuItem> MenuItems { get; } = [];

	internal MenuItem SelectedMenuItem
	{
		get => _selectedMenuItem;
		set => this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
	}

	public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }

	#endregion

	public WorkSpaceViewModel(
		IComponentContext componentContext,

		AccountService accountService,

		MenuActions menuActions,

		IReadOnlyList<IWorkSpacePage> workSpacePages)
	{
		_accountService = accountService;

		var menuItems = workSpacePages.Select(page =>
		{
			var key     = page.GetType();
			var control = componentContext.ResolveKeyed<Control>(
				key.IsGenericType ? key.GetGenericTypeDefinition() : key,
				new TypedParameter(key, page));

			return new MenuItem()
			{
				Control = control,
				Page    = page,	
			};
		});

		_accountService.AccountChange += (_, _) =>
		{
			if(_accountService.AccountUser == null) return;

			var permission = _accountService.AccountUser.Role.Permissions;

			_menuItems = [.. menuItems.Where(m => permission.Contains(m.Page.Permission))];

			WorkSpaceOwnerName = _accountService.AccountUser.Role.Description;

			ReloadMenu();
		};

		menuActions.ChangeMenu += (_, name) =>
		{
			var menu = MenuItems.FirstOrDefault(m => m.Page.Title == name);

			if(menu != null)
			{
				SelectedMenuItem = menu;
			}
		};

		ChangeThemeCommand = ReactiveCommand.Create(ChangeTheme);
	}

	private void ChangeTheme()
	{
		switch(Application.Current.RequestedThemeVariant.Key)
		{
			case "Light":
				Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
				break;
			case "Dark":
				Application.Current.RequestedThemeVariant = ThemeVariant.Light;
				break;
			default:
				// Для системной темы или других вариантов
				Application.Current.RequestedThemeVariant = ThemeVariant.Light;
				break;
		};
	}

	private void ReloadMenu()
	{
		MenuItems.Clear();

		MenuItems.AddRange(_menuItems);

		if(MenuItems.Count != 0)
		{
			SelectedMenuItem = MenuItems.First();
		}
	}
}

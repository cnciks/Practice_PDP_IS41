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

using static SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage.UserViewModel;

namespace SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;

internal class AdministrationPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly AdministrationService _service;

	private List<UserViewModel> _allUsers = [];

	private string _activeTab = "users";
	private string _searchText = string.Empty;
	private bool _isUserFormVisible;
	private bool _isRoleFormVisible;
	private bool _isUsersTab = true;
	private bool _isRolesTab;

	private string _newLogin = string.Empty;
	private string _newPassword = string.Empty;
	private string _newEmail = string.Empty;
	private RoleViewModel? _newRole;
	private EmployeeItem? _newSotrudnik;

	private string _newRoleName = string.Empty;
	private string _newRoleDescription = string.Empty;

	private string? _formError;
	private string? _roleFormError;

	#endregion

	#region Properties

	public string Title => "Администрирование";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/manager.png");

	public Permissions Permission => Permissions.AdministrationPage;

	public AvaloniaList<UserViewModel> Users { get; } = [];

	public AvaloniaList<RoleViewModel> Roles { get; } = [];

	public AvaloniaList<PermissionItemViewModel> NewRolePermissions { get; } = [];

	public AvaloniaList<EmployeeItem> Employees { get; } = [];

	public string ActiveTab
	{
		get => _activeTab;
		set
		{
			this.RaiseAndSetIfChanged(ref _activeTab, value);
			IsUsersTab = value == "users";
			IsRolesTab = value == "roles";
		}
	}

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
	}

	public bool IsUserFormVisible
	{
		get => _isUserFormVisible;
		set => this.RaiseAndSetIfChanged(ref _isUserFormVisible, value);
	}

	public bool IsRoleFormVisible
	{
		get => _isRoleFormVisible;
		set => this.RaiseAndSetIfChanged(ref _isRoleFormVisible, value);
	}

	public bool IsAnyFormVisible => _isUserFormVisible || _isRoleFormVisible;

	public bool IsUsersTab
	{
		get => _isUsersTab;
		set => this.RaiseAndSetIfChanged(ref _isUsersTab, value);
	}

	public bool IsRolesTab
	{
		get => _isRolesTab;
		set => this.RaiseAndSetIfChanged(ref _isRolesTab, value);
	}

	public string NewLogin
	{
		get => _newLogin;
		set => this.RaiseAndSetIfChanged(ref _newLogin, value);
	}

	public string NewPassword
	{
		get => _newPassword;
		set => this.RaiseAndSetIfChanged(ref _newPassword, value);
	}

	public string NewEmail
	{
		get => _newEmail;
		set => this.RaiseAndSetIfChanged(ref _newEmail, value);
	}

	public RoleViewModel? NewRole
	{
		get => _newRole;
		set => this.RaiseAndSetIfChanged(ref _newRole, value);
	}

	public EmployeeItem? NewSotrudnik
	{
		get => _newSotrudnik;
		set => this.RaiseAndSetIfChanged(ref _newSotrudnik, value);
	}

	public string NewRoleName
	{
		get => _newRoleName;
		set => this.RaiseAndSetIfChanged(ref _newRoleName, value);
	}

	public string NewRoleDescription
	{
		get => _newRoleDescription;
		set => this.RaiseAndSetIfChanged(ref _newRoleDescription, value);
	}

	public string? FormError
	{
		get => _formError;
		set => this.RaiseAndSetIfChanged(ref _formError, value);
	}

	public string? RoleFormError
	{
		get => _roleFormError;
		set => this.RaiseAndSetIfChanged(ref _roleFormError, value);
	}

	public ReactiveCommand<Unit, Unit> AddUserCommand { get; }

	public ReactiveCommand<Unit, Unit> CancelFormCommand { get; }

	public ReactiveCommand<Unit, Task> SaveUserCommand { get; }

	public ReactiveCommand<UserViewModel?, Task> DeleteUserCommand { get; }

	public ReactiveCommand<string, Unit> SwitchTabCommand { get; }

	public ReactiveCommand<Unit, Unit> AddRoleCommand { get; }

	public ReactiveCommand<Unit, Unit> CancelRoleFormCommand { get; }

	public ReactiveCommand<Unit, Task> SaveRoleCommand { get; }

	public ReactiveCommand<RoleViewModel?, Task> DeleteRoleCommand { get; }

	#endregion

	#region .ctor

	public AdministrationPageViewModel(AdministrationService service)
	{
		_service = service;

		AddUserCommand = ReactiveCommand.Create(OpenUserForm);
		CancelFormCommand = ReactiveCommand.Create(CloseUserForm);
		SaveUserCommand = ReactiveCommand.Create(SaveUser);
		DeleteUserCommand = ReactiveCommand.Create<UserViewModel?, Task>(DeleteUser);
		SwitchTabCommand = ReactiveCommand.Create<string>(SwitchTab);
		AddRoleCommand = ReactiveCommand.Create(OpenRoleForm);
		CancelRoleFormCommand = ReactiveCommand.Create(CloseRoleForm);
		SaveRoleCommand = ReactiveCommand.Create(SaveRole);
		DeleteRoleCommand = ReactiveCommand.Create<RoleViewModel?, Task>(DeleteRole);

		NewRolePermissions.AddRange(
			AccountService.AllPermissionsArray
				.Select(p => PermissionItemViewModel.FromPermission(p)));

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if(e.PropertyName == nameof(SearchText))
			ApplyFilter();
	}

	private void SwitchTab(string tab)
	{
		ActiveTab = tab;
		IsUserFormVisible = false;
		IsRoleFormVisible = false;
		this.RaisePropertyChanged(nameof(IsAnyFormVisible));
	}

	private void ApplyFilter()
	{
		var filtered = _allUsers.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(u =>
				u.Login.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				u.RoleName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Users.Clear();
		Users.AddRange(filtered);
	}

	private async void OpenUserForm()
	{
		NewLogin = string.Empty;
		NewPassword = string.Empty;
		NewEmail = string.Empty;
		NewRole = Roles.FirstOrDefault();
		NewSotrudnik = null;
		FormError = null;
		IsRoleFormVisible = false;
		IsUserFormVisible = true;
		this.RaisePropertyChanged(nameof(IsAnyFormVisible));

		try
		{
			var employees = await _service.GetAllEmployeesAsync();
			Employees.Clear();
			Employees.AddRange(employees.Select(EmployeeItem.FromDto));
		}
		catch(Exception) { }
	}

	private void CloseUserForm()
	{
		IsUserFormVisible = false;
		FormError = null;
		this.RaisePropertyChanged(nameof(IsAnyFormVisible));
	}

	private void OpenRoleForm()
	{
		NewRoleName = string.Empty;
		NewRoleDescription = string.Empty;
		RoleFormError = null;
		foreach(var p in NewRolePermissions) p.IsChecked = false;
		IsUserFormVisible = false;
		IsRoleFormVisible = true;
		this.RaisePropertyChanged(nameof(IsAnyFormVisible));
	}

	private void CloseRoleForm()
	{
		IsRoleFormVisible = false;
		RoleFormError = null;
		this.RaisePropertyChanged(nameof(IsAnyFormVisible));
	}

	private async Task SaveUser()
	{
		FormError = null;

		if(string.IsNullOrWhiteSpace(NewLogin)) { FormError = "Введите логин"; return; }
		if(string.IsNullOrWhiteSpace(NewPassword)) { FormError = "Введите пароль"; return; }
		if(NewRole is null) { FormError = "Выберите роль"; return; }

		if(_allUsers.Any(u => u.Login.Equals(NewLogin, StringComparison.OrdinalIgnoreCase)))
		{
			FormError = "Пользователь с таким логином уже существует";
			return;
		}

		try
		{
			await _service.CreateUserAsync(NewLogin, NewPassword, NewEmail, NewRole.Id, NewSotrudnik?.SotrudnikID);
			CloseUserForm();
			await ReloadUsersAsync();
		}
		catch(Exception ex)
		{
			FormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task SaveRole()
	{
		RoleFormError = null;

		if(string.IsNullOrWhiteSpace(NewRoleName)) { RoleFormError = "Введите название роли"; return; }

		if(Roles.Any(r => r.Name.Equals(NewRoleName, StringComparison.OrdinalIgnoreCase)))
		{
			RoleFormError = "Роль с таким названием уже существует";
			return;
		}

		var selectedPerms = NewRolePermissions
			.Where(p => p.IsChecked)
			.Select(p => p.Permission)
			.ToList();

		try
		{
			await _service.CreateRoleAsync(NewRoleName, NewRoleDescription, selectedPerms);
			CloseRoleForm();
			await ReloadRolesAsync();
		}
		catch(Exception ex)
		{
			RoleFormError = $"Ошибка: {ex.Message}";
		}
	}

	private async Task DeleteRole(RoleViewModel? vm)
	{
		if(vm is null) return;

		try
		{
			var deleted = await _service.DeleteRoleAsync(vm.Id);
			if(!deleted) return;
			await ReloadRolesAsync();
		}
		catch(Exception) { }
	}

	private async Task DeleteUser(UserViewModel? vm)
	{
		if(vm is null) return;

		try
		{
			await _service.DeleteUserAsync(vm.Id);
			_allUsers.Remove(vm);
			ApplyFilter();
		}
		catch(Exception) { }
	}

	private async Task ReloadUsersAsync()
	{
		try
		{
			var dtos = await _service.GetAllUsersAsync();
			_allUsers = dtos.Select(u => new UserViewModel
			{
				Id = u.Id,
				Login = u.Login,
				Email = u.Email,
				RoleName = u.Role?.Name ?? "—",
				RoleId = u.Role?.Id ?? 0,
				SotrudnikFIO = u.SotrudnikFIO,
			}).ToList();
			ApplyFilter();
		}
		catch(Exception) { }
	}

	private async Task ReloadRolesAsync()
	{
		try
		{
			var dtos = await _service.GetAllRolesAsync();
			var usersByRole = _allUsers.GroupBy(u => u.RoleId)
				.ToDictionary(g => g.Key, g => g.Count());

			Roles.Clear();
			Roles.AddRange(dtos.Select(r =>
			{
				usersByRole.TryGetValue(r.Id, out var count);
				return new RoleViewModel
				{
					Id = r.Id,
					Name = r.Name,
					Description = r.Description,
					Permissions = r.Permissions,
					UserCount = count,
				};
			}));
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		await ReloadUsersAsync();
		await ReloadRolesAsync();
	}
}

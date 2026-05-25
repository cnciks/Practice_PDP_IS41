using ReactiveUI;

using SchoolAssistancePlatform.UI.Services;
using SchoolAssistancePlatform.UI.Views.Auth;
using SchoolAssistancePlatform.UI.Views.WorkSpace;

namespace SchoolAssistancePlatform.UI.Windows;

internal class MainWindowViewModel : ReactiveObject
{
	private readonly AccountService _accountService;

	private bool _isAuth = false;

	public string Title => "ИЦП";

	public WorkSpaceViewModel WorkSpace { get; }

	public AuthViewModel Auth { get; }

	public bool IsAuth
	{
		get => _isAuth;
		set => this.RaiseAndSetIfChanged(ref _isAuth, value);
	}

	public MainWindowViewModel(
		AccountService accountService,

		WorkSpaceViewModel workSpaceViewModel,
		AuthViewModel authViewModel)
	{
		_accountService = accountService;

		WorkSpace = workSpaceViewModel;
		Auth      = authViewModel;

		_accountService.AccountChange += (_, _) =>
		{
			IsAuth = _accountService.IsLogIn;
		};
	}
}

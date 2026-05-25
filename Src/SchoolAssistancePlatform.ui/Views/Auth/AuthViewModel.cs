using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Auth;

internal partial class AuthViewModel : ReactiveObject
{

	private AccountService _accountService;

	private string _login;

	private string _password;

	public string Login
	{
		get => _login;
		set => this.RaiseAndSetIfChanged(ref _login, value);
	}

	public string Password
	{
		get => _password;
		set => this.RaiseAndSetIfChanged(ref _password, value);
	}

	public ReactiveCommand<Unit, Unit> LoginCommand { get; }

	public AuthViewModel(AccountService accountService)
	{
		_accountService = accountService;

		LoginCommand = ReactiveCommand.CreateFromTask(OnLoginAsync);


#if DEBUG

		_login = "admin";

		_password = "admin";

#endif
	}

	private async Task OnLoginAsync()
	{
		var correct = await _accountService.CheckUserAsync(_login, _password);

		if(correct)
		{
			await _accountService.ChangeAccountAsync(_login, _password);

			Login    = string.Empty;
			Password = string.Empty;
		}
	}
}

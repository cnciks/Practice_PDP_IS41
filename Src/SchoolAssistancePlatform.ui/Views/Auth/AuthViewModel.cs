using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Auth;

internal partial class AuthViewModel : ReactiveObject
{
	private readonly AccountService _accountService;

	private string  _login    = string.Empty;
	private string  _password = string.Empty;
	private string? _error;
	private bool    _isLoading;

	public string Login
	{
		get => _login;
		set
		{
			this.RaiseAndSetIfChanged(ref _login, value);
			Error = null;
		}
	}

	public string Password
	{
		get => _password;
		set
		{
			this.RaiseAndSetIfChanged(ref _password, value);
			Error = null;
		}
	}

	public string? Error
	{
		get => _error;
		set => this.RaiseAndSetIfChanged(ref _error, value);
	}

	public bool IsLoading
	{
		get => _isLoading;
		set => this.RaiseAndSetIfChanged(ref _isLoading, value);
	}

	public ReactiveCommand<Unit, Unit> LoginCommand { get; }

	public AuthViewModel(AccountService accountService)
	{
		_accountService = accountService;

		LoginCommand = ReactiveCommand.CreateFromTask(OnLoginAsync);

#if DEBUG
		_login    = "admin";
		_password = "admin";
#endif
	}

	private async Task OnLoginAsync()
	{
		Error = null;

		if(string.IsNullOrWhiteSpace(_login))
		{
			Error = "Введите логин";
			return;
		}

		if(string.IsNullOrWhiteSpace(_password))
		{
			Error = "Введите пароль";
			return;
		}

		IsLoading = true;
		try
		{
			var correct = await _accountService.CheckUserAsync(_login, _password);

			if(!correct)
			{
				Error = "Неверный логин или пароль";
				return;
			}

			await _accountService.ChangeAccountAsync(_login, _password);

			Login    = string.Empty;
			Password = string.Empty;
		}
		catch
		{
			Error = "Ошибка подключения к базе данных";
		}
		finally
		{
			IsLoading = false;
		}
	}
}

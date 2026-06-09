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

namespace SchoolAssistancePlatform.UI.Views.Pages.MessagesPage;

internal class MessagesPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly MessagesService _service;
	private readonly AccountService  _account;

	private List<MessageViewModel> _allMessages = [];

	private string           _searchText      = string.Empty;
	private string           _activeTab        = "inbox";
	private MessageViewModel? _selectedMessage;
	private bool              _isDetailsVisible;
	private bool              _isComposeVisible;

	#endregion

	#region Properties

	public string Title      => "Сообщения";
	public Bitmap?     Icon      => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/open-email.png");
	public Permissions Permission => Permissions.MessagesPage;

	public AvaloniaList<MessageViewModel> Messages { get; } = [];

	public ComposeViewModel Compose { get; } = new();

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
	}

	public string ActiveTab
	{
		get => _activeTab;
		set => this.RaiseAndSetIfChanged(ref _activeTab, value);
	}

	public MessageViewModel? SelectedMessage
	{
		get => _selectedMessage;
		set => this.RaiseAndSetIfChanged(ref _selectedMessage, value);
	}

	public bool IsDetailsVisible
	{
		get => _isDetailsVisible;
		set => this.RaiseAndSetIfChanged(ref _isDetailsVisible, value);
	}

	public bool IsComposeVisible
	{
		get => _isComposeVisible;
		set => this.RaiseAndSetIfChanged(ref _isComposeVisible, value);
	}

	public ReactiveCommand<Unit, Unit>              ComposeCommand       { get; }
	public ReactiveCommand<Unit, Unit>              CancelComposeCommand { get; }
	public ReactiveCommand<Unit, Task>              SendCommand          { get; }
	public ReactiveCommand<MessageViewModel?, Unit> OpenMessageCommand   { get; }
	public ReactiveCommand<Unit, Unit>              CloseMessageCommand  { get; }
	public ReactiveCommand<string, Task>            SwitchTabCommand     { get; }
	public ReactiveCommand<MessageViewModel?, Task> DeleteCommand        { get; }

	#endregion

	#region .ctor

	public MessagesPageViewModel(MessagesService service, AccountService account)
	{
		_service = service;
		_account = account;

		ComposeCommand       = ReactiveCommand.Create(OpenCompose);
		CancelComposeCommand = ReactiveCommand.Create(CloseCompose);
		SendCommand          = ReactiveCommand.Create(SendMessage);
		OpenMessageCommand   = ReactiveCommand.Create<MessageViewModel?>(OpenMessage);
		CloseMessageCommand  = ReactiveCommand.Create(CloseMessage);
		SwitchTabCommand     = ReactiveCommand.Create<string, Task>(SwitchTab);
		DeleteCommand        = ReactiveCommand.Create<MessageViewModel?, Task>(DeleteMessage);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if(e.PropertyName == nameof(SearchText))
			ApplyFilter();
	}

	private long CurrentUserID => _account.AccountUser?.Id ?? 0;

	private async Task LoadMessagesForTab(string tab)
	{
		try
		{
			IEnumerable<SoobschenieDto> raw = tab switch
			{
				"outbox" => await _service.GetOutboxAsync(CurrentUserID),
				"unread" => await _service.GetUnreadAsync(CurrentUserID),
				_        => await _service.GetInboxAsync(CurrentUserID),
			};

			_allMessages = raw.OrderByDescending(m => m.DataOtpravki).Select(dto => new MessageViewModel
			{
				SoobschenieID   = dto.SoobschenieID,
				OtpravitelID    = dto.OtpravitelID,
				PoluchatelID    = dto.PoluchatelID,
				OtpravitelFIO   = dto.OtpravitelFIO,
				PoluchatelFIO   = dto.PoluchatelFIO,
				TipPoluchatelya = dto.TipPoluchatelya,
				Tema            = dto.Tema,
				Text            = dto.Text,
				DataOtpravki    = dto.DataOtpravki,
				Prochitano      = dto.Prochitano,
			}).ToList();
		}
		catch(Exception)
		{
			_allMessages = [];
		}

		ApplyFilter();
	}

	private void ApplyFilter()
	{
		var filtered = _allMessages.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(m =>
				m.Tema.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				m.OtpravitelFIO.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				m.PoluchatelFIO.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				m.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Messages.Clear();
		Messages.AddRange(filtered);
	}

	private async void OpenMessage(MessageViewModel? vm)
	{
		if(vm is null) return;
		SelectedMessage  = vm;
		IsDetailsVisible = true;
		IsComposeVisible = false;

		if(!vm.Prochitano)
		{
			await _service.MarkAsReadAsync(vm.SoobschenieID);
			vm.Prochitano = true;
		}
	}

	private void CloseMessage()
	{
		IsDetailsVisible = false;
		SelectedMessage  = null;
	}

	private async void OpenCompose()
	{
		IsComposeVisible = true;
		IsDetailsVisible = false;

		try
		{
			var employees = await _service.GetAllEmployeesAsync();
			Compose.Recipients = employees
				.Select(SotrudnikItem.FromDto)
				.OrderBy(s => s.FullName)
				.ToList();
			Compose.SelectedRecipient = null;
			Compose.Tema = string.Empty;
			Compose.Text = string.Empty;
		}
		catch(Exception) { }
	}

	private void CloseCompose()
	{
		IsComposeVisible = false;
	}

	private async Task SendMessage()
	{
		if(!Compose.IsValid) return;

		try
		{
			var dto = new SoobschenieDto
			{
				OtpravitelID    = CurrentUserID,
				PoluchatelID    = Compose.SelectedRecipient!.SotrudnikID,
				TipPoluchatelya = "Сотрудник",
				Tema            = Compose.Tema,
				Text            = Compose.Text,
				DataOtpravki    = DateTime.Now,
				Prochitano      = false,
			};

			await _service.SendAsync(dto);
			CloseCompose();

			if(ActiveTab == "outbox")
				await LoadMessagesForTab("outbox");
		}
		catch(Exception) { }
	}

	private async Task SwitchTab(string tab)
	{
		ActiveTab        = tab;
		IsDetailsVisible = false;
		SelectedMessage  = null;
		await LoadMessagesForTab(tab);
	}

	private async Task DeleteMessage(MessageViewModel? vm)
	{
		if(vm is null) return;

		try
		{
			await _service.DeleteAsync(vm.SoobschenieID);

			if(SelectedMessage == vm)
			{
				IsDetailsVisible = false;
				SelectedMessage  = null;
			}

			_allMessages.Remove(vm);
			ApplyFilter();
		}
		catch(Exception) { }
	}

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadMessagesForTab("inbox");
	}
}

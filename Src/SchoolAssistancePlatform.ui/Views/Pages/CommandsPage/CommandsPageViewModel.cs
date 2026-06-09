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

namespace SchoolAssistancePlatform.UI.Views.Pages.CommandsPage;

internal class CommandsPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly CommandsService _service;

	private List<PrikazViewModel> _allItems = [];

	private string? _selectedType;
	private string  _searchText  = string.Empty;
	private bool    _applyFilter;

	private PrikazViewModel? _selectedPrikaz;
	private bool             _isDetailsVisible;

	#endregion

	#region Properties

	public string Title => "Приказы";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/studying.png");

	public Permissions Permission => Permissions.CommandsPage;

	public AvaloniaList<PrikazViewModel> Documents { get; } = [];

	public AvaloniaList<string> Types { get; } = [];

	public string? SelectedType
	{
		get => _selectedType;
		set => this.RaiseAndSetIfChanged(ref _selectedType, value);
	}

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
	}

	public bool ApplyFilter
	{
		get => _applyFilter;
		set => this.RaiseAndSetIfChanged(ref _applyFilter, value);
	}

	public PrikazViewModel? SelectedPrikaz
	{
		get => _selectedPrikaz;
		set => this.RaiseAndSetIfChanged(ref _selectedPrikaz, value);
	}

	public bool IsDetailsVisible
	{
		get => _isDetailsVisible;
		set => this.RaiseAndSetIfChanged(ref _isDetailsVisible, value);
	}

	public ReactiveCommand<Unit, Task> CreateCommand { get; }

	public ReactiveCommand<Unit, Task> ExportCommand { get; }

	public ReactiveCommand<PrikazViewModel?, Unit> SelectPrikazCommand { get; }

	public ReactiveCommand<Unit, Unit> CloseDetailsCommand { get; }

	#endregion

	#region .ctor

	public CommandsPageViewModel(CommandsService service)
	{
		_service = service;

		CreateCommand       = ReactiveCommand.Create(Create);
		ExportCommand       = ReactiveCommand.Create(Export);
		SelectPrikazCommand = ReactiveCommand.Create<PrikazViewModel?>(SelectPrikaz);
		CloseDetailsCommand = ReactiveCommand.Create(CloseDetails);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(SelectedType):
			case nameof(SearchText):
				ApplyFilters();
				break;
		}
	}

	private async Task LoadData()
	{
		try
		{
			var all   = await _service.GetAllAsync();
			var types = await _service.GetAvailableTypesAsync();

			_allItems = all.Select(p => new PrikazViewModel
			{
				PrikazID    = p.PrikazID,
				Nomer       = p.NomerPrikaza,
				Date        = p.DataPrikaza,
				Tip         = p.TipPrikaza,
				Soderzhanie = p.Soderzhanie,
				Sotrudnik   = p.SotrudnikFIO,
				FileLink    = p.FileLink,
			}).ToList();

			Types.Clear();
			Types.AddRange(types);

			ApplyFilters();
		}
		catch(Exception)
		{
		}
	}

	private void ApplyFilters()
	{
		var filtered = _allItems.AsEnumerable();

		if(!string.IsNullOrWhiteSpace(SelectedType))
			filtered = filtered.Where(p => p.Tip == SelectedType);

		if(!string.IsNullOrWhiteSpace(SearchText))
			filtered = filtered.Where(p =>
				p.Nomer.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				p.Soderzhanie.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				p.Sotrudnik.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		Documents.Clear();
		Documents.AddRange(filtered);
	}

	private void SelectPrikaz(PrikazViewModel? vm)
	{
		if(vm is null) return;
		SelectedPrikaz   = vm;
		IsDetailsVisible = true;
	}

	private void CloseDetails()
	{
		IsDetailsVisible = false;
		SelectedPrikaz   = null;
	}

	private Task Create() => Task.CompletedTask;
	private Task Export() => Task.CompletedTask;

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadData();
	}
}

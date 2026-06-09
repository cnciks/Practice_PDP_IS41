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

namespace SchoolAssistancePlatform.UI.Views.Pages.FinancePage;

internal class FinancePageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly FinanceService _service;

	private int?   _selectedYear;
	private bool   _applyFilter;
	private string _searchText = string.Empty;

	private List<TransactionViewModel> _allTransactions = [];

	#endregion

	#region Properties

	public string Title => "Финансы";

	public Bitmap? Icon => MenuIcon.Load("avares://SchoolAssistancePlatform.ui/Assets/Images/math-book.png");

	public Permissions Permission => Permissions.FinancePage;

	public AvaloniaList<TransactionViewModel> Transactions { get; } = [];

	public AvaloniaList<SummaryCardViewModel> SummaryCards { get; } = [];

	public AvaloniaList<int> Years { get; } = [];

	public int? SelectedYear
	{
		get => _selectedYear;
		set => this.RaiseAndSetIfChanged(ref _selectedYear, value);
	}

	public bool ApplyFilter
	{
		get => _applyFilter;
		set => this.RaiseAndSetIfChanged(ref _applyFilter, value);
	}

	public string SearchText
	{
		get => _searchText;
		set => this.RaiseAndSetIfChanged(ref _searchText, value);
	}

	public ReactiveCommand<Unit, Task> ExportCommand { get; }
	public ReactiveCommand<Unit, Task> ReportCommand { get; }

	#endregion

	#region .ctor

	public FinancePageViewModel(FinanceService service)
	{
		_service = service;

		ExportCommand = ReactiveCommand.Create(Export);
		ReportCommand = ReactiveCommand.Create(Report);

		PropertyChanged += OnPropertyChanged;
	}

	#endregion

	#region Private methods

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch(e.PropertyName)
		{
			case nameof(SelectedYear):
				_ = ReloadAsync();
				break;
			case nameof(SearchText):
				ApplySearch();
				break;
		}
	}

	private async Task LoadData()
	{
		try
		{
			var years = (await _service.GetAvailableYearsAsync()).ToList();

			if(!years.Contains(DateTime.Now.Year))
				years.Insert(0, DateTime.Now.Year);

			Years.Clear();
			Years.AddRange(years);
			SelectedYear = years.FirstOrDefault();

			await ReloadAsync();
		}
		catch(Exception)
		{
		}
	}

	private async Task ReloadAsync()
	{
		try
		{
			var items = SelectedYear.HasValue
				? await _service.GetByYearAsync(SelectedYear.Value)
				: await _service.GetAllAsync();

			_allTransactions = items.Select(f => new TransactionViewModel
			{
				PlatezhID   = f.PlatezhID,
				Student     = f.FIOUchenika,
				Klass       = f.NomerKlassa,
				Date        = f.DataPlatezha,
				Naznachenie = f.Naznachenie,
				TipOperacii = f.TipOperacii,
				Summa       = f.Summa,
			}).ToList();

			ApplySearch();
			RebuildSummary(_allTransactions);
		}
		catch(Exception)
		{
		}
	}

	private void ApplySearch()
	{
		var filtered = string.IsNullOrWhiteSpace(SearchText)
			? _allTransactions
			: _allTransactions.Where(t =>
				t.Student.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				t.Naznachenie.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				t.TipOperacii.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
				t.Klass.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

		Transactions.Clear();
		Transactions.AddRange(filtered);
	}

	private void RebuildSummary(List<TransactionViewModel> items)
	{
		var income   = items.Where(t => t.IsIncome).Sum(t => t.Summa);
		var expense  = items.Where(t => !t.IsIncome).Sum(t => t.Summa);
		var balance  = income - expense;

		SummaryCards.Clear();

		SummaryCards.Add(new SummaryCardViewModel
		{
			Label  = "Доходы",
			Amount = income,
			Color  = "#27AE60",
			Icon   = "📈",
		});

		SummaryCards.Add(new SummaryCardViewModel
		{
			Label  = "Расходы",
			Amount = expense,
			Color  = "#E74C3C",
			Icon   = "📉",
		});

		SummaryCards.Add(new SummaryCardViewModel
		{
			Label  = "Баланс",
			Amount = balance,
			Color  = balance >= 0 ? "#3498DB" : "#E67E22",
			Icon   = "💰",
		});
	}

	private Task Export() => Task.CompletedTask;

	private Task Report()  => Task.CompletedTask;

	#endregion

	public async Task LoadPageAsync()
	{
		await LoadData();
	}
}

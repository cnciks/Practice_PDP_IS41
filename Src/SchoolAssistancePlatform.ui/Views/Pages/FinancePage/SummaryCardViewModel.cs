namespace SchoolAssistancePlatform.UI.Views.Pages.FinancePage;

internal sealed class SummaryCardViewModel
{
	public string Label { get; init; } = string.Empty;

	public decimal Amount { get; init; }

	public string AmountText => $"{Amount:N2} ₽";

	public string Color { get; init; } = "#555";

	public string Icon { get; init; } = string.Empty;
}

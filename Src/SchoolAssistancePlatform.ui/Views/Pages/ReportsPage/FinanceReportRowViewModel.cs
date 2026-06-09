namespace SchoolAssistancePlatform.UI.Views.Pages.ReportsPage;

internal sealed class FinanceReportRowViewModel
{
	public string  Month    { get; init; } = string.Empty;
	public decimal Income   { get; init; }
	public decimal Expense  { get; init; }
	public decimal Balance  => Income - Expense;

	public string IncomeText  => $"+{Income:N0} ₽";
	public string ExpenseText => $"-{Expense:N0} ₽";
	public string BalanceText => Balance >= 0 ? $"+{Balance:N0} ₽" : $"{Balance:N0} ₽";
	public string BalanceColor => Balance >= 0 ? "#27AE60" : "#E74C3C";
}

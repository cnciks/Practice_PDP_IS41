namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal sealed class PeriodItem(string label, int monthsBack)
{
	public string Label      { get; } = label;
	public int    MonthsBack { get; } = monthsBack;
}

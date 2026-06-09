using System;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal sealed class DateHeaderViewModel
{
	public DateTime Date { get; init; }
	public string   Day  => Date.ToString("dd.MM");
}

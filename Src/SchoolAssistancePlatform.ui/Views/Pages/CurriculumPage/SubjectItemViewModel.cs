namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class SubjectItemViewModel
{
	public long   PredmetID    { get; init; }
	public string Nazvanie     { get; init; } = string.Empty;
	public string Sokrashenie  { get; init; } = string.Empty;
	public int    ChasovNedelyu { get; init; }
}

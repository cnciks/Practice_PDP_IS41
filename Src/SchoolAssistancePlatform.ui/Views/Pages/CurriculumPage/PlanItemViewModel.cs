namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class PlanItemViewModel
{
	public long   PlanID     { get; init; }
	public string Nazvanie   { get; init; } = string.Empty;
	public int    GodNachala { get; init; }
	public string Opisanie   { get; init; } = string.Empty;
}

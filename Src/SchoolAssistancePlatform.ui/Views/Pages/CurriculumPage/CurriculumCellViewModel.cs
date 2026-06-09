namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class CurriculumCellViewModel
{
	public string Text            { get; init; } = string.Empty;
	public bool   IsOver          { get; init; }
	public bool   HasLessons      { get; init; }
	public string ForegroundColor => IsOver ? "#E74C3C" : HasLessons ? "#2C3E50" : "#B0B0B0";
}

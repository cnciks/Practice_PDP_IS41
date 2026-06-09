namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

/// <summary>
/// Строка таблицы расписания: один номер урока + ячейки для каждого дня недели.
/// </summary>
internal sealed class ScheduleRowViewModel
{
	public string Time { get; init; } = string.Empty;

	public LessonCellViewModel Monday    { get; init; } = new();
	public LessonCellViewModel Tuesday   { get; init; } = new();
	public LessonCellViewModel Wednesday { get; init; } = new();
	public LessonCellViewModel Thursday  { get; init; } = new();
	public LessonCellViewModel Friday    { get; init; } = new();
}

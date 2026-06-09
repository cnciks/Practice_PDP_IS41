namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal sealed class LessonCellViewModel
{
	public string? Subject { get; init; }
	public string? Teacher { get; init; }
	public string? Room    { get; init; }

	public long? RaspisanieID { get; init; }
	public long  DenNedeli    { get; init; }
	public short NomerUroka   { get; init; }
	public long? KlassID      { get; init; }
	public long? SotrudnikID  { get; init; }
	public long? PredmetID    { get; init; }
	public string? NomerKlassa { get; init; }

	public bool HasLesson => Subject is not null;
}

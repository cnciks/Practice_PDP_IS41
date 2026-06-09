using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

internal sealed class ClassItemViewModel : ReactiveObject
{
	public long   KlassID       { get; init; }
	public string Name          { get; init; } = string.Empty;
	public int    GodObucheniya { get; init; }
	public long   TeacherId     { get; init; }
	public long   PlanID        { get; init; }
	public string Teacher       { get; init; } = string.Empty;
	public int    StudentsCount { get; init; }
	public string PlanName      { get; init; } = string.Empty;

	public string BadgeColor => (Name.Length > 0 ? Name[0] : '0') switch
	{
		'1' or '2' or '3' or '4' => "#3498DB",
		'5' or '6'               => "#9B59B6",
		'7' or '8'               => "#27AE60",
		'9'                      => "#E67E22",
		_                        => "#E74C3C",
	};

	public static ClassItemViewModel FromData(KlassDto dto, int studentsCount, string teacher) => new()
	{
		KlassID       = dto.KlassID,
		Name          = dto.NomerKlassa ?? string.Empty,
		GodObucheniya = dto.GodObucheniya,
		TeacherId     = dto.KlassRukovoditelID,
		PlanID        = dto.PlanID,
		Teacher       = teacher,
		StudentsCount = studentsCount,
		PlanName      = dto.PlanNazvanie ?? string.Empty,
	};
}

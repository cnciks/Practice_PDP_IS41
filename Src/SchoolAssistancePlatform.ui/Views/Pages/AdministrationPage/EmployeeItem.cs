using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;

internal sealed class EmployeeItem
{
	public long   SotrudnikID { get; init; }
	public string FullName    { get; init; } = string.Empty;
	public string Dolzhnost   { get; init; } = string.Empty;

	public string DisplayText => string.IsNullOrWhiteSpace(Dolzhnost)
		? FullName
		: $"{FullName} ({Dolzhnost})";

	public static EmployeeItem FromDto(SotrudnikDto dto) => new()
	{
		SotrudnikID = dto.SotrudnikID,
		FullName    = $"{dto.Familia} {dto.Imya} {dto.Otchestvo}".Trim(),
		Dolzhnost   = dto.Dolzhnost ?? string.Empty,
	};
}

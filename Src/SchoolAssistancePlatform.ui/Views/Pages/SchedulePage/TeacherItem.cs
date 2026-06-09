using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal sealed class TeacherItem
{
	public long   SotrudnikID { get; init; }
	public string FullName    { get; init; } = string.Empty;

	public static TeacherItem FromDto(SotrudnikDto dto) => new()
	{
		SotrudnikID = dto.SotrudnikID,
		FullName    = string.IsNullOrEmpty(dto.Otchestvo)
			? $"{dto.Familia} {dto.Imya}"
			: $"{dto.Familia} {dto.Imya} {dto.Otchestvo}",
	};
}

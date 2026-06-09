using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal sealed class UchebniyPredmetItem
{
	public long   PredmetID { get; init; }
	public string Nazvanie  { get; init; } = string.Empty;

	public static UchebniyPredmetItem FromDto(UchebniyPredmetDto dto) => new()
	{
		PredmetID = dto.PredmetID,
		Nazvanie  = dto.Nazvanie,
	};
}

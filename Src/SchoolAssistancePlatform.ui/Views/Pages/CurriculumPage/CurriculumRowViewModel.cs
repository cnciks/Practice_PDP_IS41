using System.Collections.Generic;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class CurriculumRowViewModel
{
	public long PredmetID { get; init; }

	public string Subject { get; init; } = string.Empty;

	public string Abbr { get; init; } = string.Empty;

	/// <summary>
	/// Часов в неделю по предмету из UchebniyPredmetEntity (норматив).
	/// </summary>
	public int NormHours { get; init; }

	/// <summary>
	/// Ключ = KlassID, значение = кол-во уроков в неделю из расписания.
	/// </summary>
	public Dictionary<long, int> HoursByKlass { get; init; } = [];
}

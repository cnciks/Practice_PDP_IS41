using System.Collections.Generic;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

/// <summary>
/// Готовая строка для отображения в ListBox.
/// Cells — упорядоченный список ячеек (по одной на класс).
/// </summary>
internal sealed class CurriculumFlatRowViewModel
{
	public string Subject { get; init; } = string.Empty;

	public string Abbr { get; init; } = string.Empty;

	public int NormHours { get; init; }

	public List<CurriculumCellViewModel> Cells { get; init; } = [];
}

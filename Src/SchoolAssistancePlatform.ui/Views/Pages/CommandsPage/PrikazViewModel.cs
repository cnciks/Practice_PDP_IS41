using System;

namespace SchoolAssistancePlatform.UI.Views.Pages.CommandsPage;

internal sealed class PrikazViewModel
{
	public int     PrikazID    { get; init; }
	public string  Nomer       { get; init; } = string.Empty;
	public DateTime Date       { get; init; }
	public string  DateText    => Date.ToString("dd.MM.yyyy");
	public string  Tip         { get; init; } = string.Empty;
	public string  Soderzhanie { get; init; } = string.Empty;
	public string  Sotrudnik   { get; init; } = string.Empty;
	public string? FileLink    { get; init; }

	public bool   HasFile     => !string.IsNullOrWhiteSpace(FileLink);

	public string BadgeColor  => Tip switch
	{
		"Зачисление"  => "#3498DB",
		"Перевод"     => "#9B59B6",
		"Отчисление"  => "#E74C3C",
		"Поощрение"   => "#27AE60",
		"Взыскание"   => "#E67E22",
		_             => "#7F8C8D",
	};
}

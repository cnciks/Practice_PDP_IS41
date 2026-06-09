using System;

namespace SchoolAssistancePlatform.UI.Views.Pages.MainPage;

internal sealed class RecentPrikazViewModel
{
	public string  Nomer       { get; init; } = string.Empty;
	public string  Tip         { get; init; } = string.Empty;
	public string  Soderzhanie { get; init; } = string.Empty;
	public DateTime Date       { get; init; }

	public string DateText   => Date.ToString("dd.MM.yyyy");
	public string BadgeColor => Tip switch
	{
		"Зачисление" => "#3498DB",
		"Перевод"    => "#9B59B6",
		"Отчисление" => "#E74C3C",
		"Поощрение"  => "#27AE60",
		"Взыскание"  => "#E67E22",
		_            => "#7F8C8D",
	};
}

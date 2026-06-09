using System;

using ReactiveUI;

namespace SchoolAssistancePlatform.UI.Views.Pages.MessagesPage;

internal sealed class MessageViewModel : ReactiveObject
{
	private bool _prochitano;

	public int      SoobschenieID  { get; init; }
	public long     OtpravitelID   { get; init; }
	public long     PoluchatelID   { get; init; }
	public string   OtpravitelFIO  { get; init; } = string.Empty;
	public string   PoluchatelFIO  { get; init; } = string.Empty;
	public string   TipPoluchatelya { get; init; } = string.Empty;
	public string   Tema           { get; init; } = string.Empty;
	public string   Text           { get; init; } = string.Empty;
	public DateTime DataOtpravki   { get; init; }

	public bool Prochitano
	{
		get => _prochitano;
		set => this.RaiseAndSetIfChanged(ref _prochitano, value);
	}

	public string DateText    => DataOtpravki.ToString("dd.MM.yyyy HH:mm");
	public string DateShort   => DataOtpravki.ToString("dd.MM");
	public string TextPreview => Text.Length > 80 ? Text[..80] + "…" : Text;

	public string FontWeight  => Prochitano ? "Normal" : "SemiBold";
	public string DotColor    => Prochitano ? "Transparent" : "#3498DB";

	public string BadgeColor  => TipPoluchatelya switch
	{
		"Учитель"   => "#3498DB",
		"Родитель"  => "#27AE60",
		"Ученик"    => "#9B59B6",
		"Директор"  => "#E74C3C",
		_           => "#7F8C8D",
	};
}

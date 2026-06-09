using System.Collections.Generic;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.MessagesPage;

internal sealed class ComposeViewModel : ReactiveObject
{
	private SotrudnikItem? _selectedRecipient;
	private string         _tema    = string.Empty;
	private string         _text    = string.Empty;

	public List<SotrudnikItem> Recipients { get; set; } = [];

	public SotrudnikItem? SelectedRecipient
	{
		get => _selectedRecipient;
		set => this.RaiseAndSetIfChanged(ref _selectedRecipient, value);
	}

	public string Tema
	{
		get => _tema;
		set => this.RaiseAndSetIfChanged(ref _tema, value);
	}

	public string Text
	{
		get => _text;
		set => this.RaiseAndSetIfChanged(ref _text, value);
	}

	public bool IsValid => SelectedRecipient is not null
		&& !string.IsNullOrWhiteSpace(Tema)
		&& !string.IsNullOrWhiteSpace(Text);
}

internal sealed class SotrudnikItem
{
	public long   SotrudnikID { get; init; }
	public string FullName    { get; init; } = string.Empty;

	public static SotrudnikItem FromDto(SotrudnikDto dto) => new()
	{
		SotrudnikID = dto.SotrudnikID,
		FullName    = $"{dto.Familia} {dto.Imya} {dto.Otchestvo}".Trim(),
	};
}

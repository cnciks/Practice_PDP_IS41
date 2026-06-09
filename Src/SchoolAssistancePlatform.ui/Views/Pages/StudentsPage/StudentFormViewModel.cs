using System;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

internal sealed class StudentFormViewModel : ReactiveObject
{
	private string          _familiia         = string.Empty;
	private string          _imya             = string.Empty;
	private string          _otchestvo        = string.Empty;
	private DateTimeOffset? _dataRozhdeniya   = DateTimeOffset.Now.AddYears(-12);
	private string          _adres            = string.Empty;
	private string          _fioRoditeley     = string.Empty;
	private string          _telefonRoditelya = string.Empty;
	private DateTimeOffset? _dataZachisleniya = DateTimeOffset.Now;
	private KlassItem?      _selectedKlass;

	public string Familiia
	{
		get => _familiia;
		set => this.RaiseAndSetIfChanged(ref _familiia, value);
	}

	public string Imya
	{
		get => _imya;
		set => this.RaiseAndSetIfChanged(ref _imya, value);
	}

	public string Otchestvo
	{
		get => _otchestvo;
		set => this.RaiseAndSetIfChanged(ref _otchestvo, value);
	}

	public DateTimeOffset? DataRozhdeniya
	{
		get => _dataRozhdeniya;
		set => this.RaiseAndSetIfChanged(ref _dataRozhdeniya, value);
	}

	public string Adres
	{
		get => _adres;
		set => this.RaiseAndSetIfChanged(ref _adres, value);
	}

	public string FIORoditeley
	{
		get => _fioRoditeley;
		set => this.RaiseAndSetIfChanged(ref _fioRoditeley, value);
	}

	public string TelefonRoditelya
	{
		get => _telefonRoditelya;
		set => this.RaiseAndSetIfChanged(ref _telefonRoditelya, value);
	}

	public DateTimeOffset? DataZachisleniya
	{
		get => _dataZachisleniya;
		set => this.RaiseAndSetIfChanged(ref _dataZachisleniya, value);
	}

	public KlassItem? SelectedKlass
	{
		get => _selectedKlass;
		set => this.RaiseAndSetIfChanged(ref _selectedKlass, value);
	}

	public bool IsValid =>
		!string.IsNullOrWhiteSpace(Familiia) &&
		!string.IsNullOrWhiteSpace(Imya);

	public void LoadFrom(UchenikDto dto, KlassItem? klass)
	{
		Familiia         = dto.Familiia         ?? string.Empty;
		Imya             = dto.Imya             ?? string.Empty;
		Otchestvo        = dto.Otchestvo        ?? string.Empty;
		DataRozhdeniya   = new DateTimeOffset(dto.DataRozhdeniya, TimeSpan.Zero);
		Adres            = dto.AdresProzhivaniya ?? string.Empty;
		FIORoditeley     = dto.FIORoditeley     ?? string.Empty;
		TelefonRoditelya = dto.TelefonRoditelya ?? string.Empty;
		DataZachisleniya = new DateTimeOffset(dto.DataZachisleniya, TimeSpan.Zero);
		SelectedKlass    = klass;
	}

	public void Reset()
	{
		Familiia         = string.Empty;
		Imya             = string.Empty;
		Otchestvo        = string.Empty;
		DataRozhdeniya   = DateTimeOffset.Now.AddYears(-12);
		Adres            = string.Empty;
		FIORoditeley     = string.Empty;
		TelefonRoditelya = string.Empty;
		DataZachisleniya = DateTimeOffset.Now;
		SelectedKlass    = null;
	}

	public UchenikDto ToDto() => new()
	{
		Familiia          = Familiia,
		Imya              = Imya,
		Otchestvo         = Otchestvo,
		DataRozhdeniya    = DataRozhdeniya?.DateTime ?? DateTime.Today.AddYears(-12),
		AdresProzhivaniya = Adres,
		FIORoditeley      = FIORoditeley,
		TelefonRoditelya  = TelefonRoditelya,
		DataZachisleniya  = DataZachisleniya?.DateTime ?? DateTime.Today,
		KlassID           = SelectedKlass?.KlassID ?? 0,
	};
}

internal sealed class KlassItem
{
	public long   KlassID     { get; init; }
	public string NomerKlassa { get; init; } = string.Empty;

	public override string ToString() => NomerKlassa;

	public static KlassItem FromDto(KlassDto dto) => new()
	{
		KlassID     = dto.KlassID,
		NomerKlassa = dto.NomerKlassa ?? string.Empty,
	};
}

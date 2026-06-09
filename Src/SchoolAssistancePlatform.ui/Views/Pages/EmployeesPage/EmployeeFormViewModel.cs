using System;

using ReactiveUI;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal sealed class EmployeeFormViewModel : ReactiveObject
{
	private string          _familia        = string.Empty;
	private string          _imya           = string.Empty;
	private string          _otchestvo      = string.Empty;
	private DateTimeOffset? _dataRozhdeniya = DateTimeOffset.Now.AddYears(-30);
	private string          _dolzhnost      = string.Empty;
	private string          _email          = string.Empty;
	private string          _telefon        = string.Empty;
	private DateTimeOffset? _dataPriema     = DateTimeOffset.Now;
	private string          _status         = "Активен";

	public string Familia
	{
		get => _familia;
		set => this.RaiseAndSetIfChanged(ref _familia, value);
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

	public string Dolzhnost
	{
		get => _dolzhnost;
		set => this.RaiseAndSetIfChanged(ref _dolzhnost, value);
	}

	public string Email
	{
		get => _email;
		set => this.RaiseAndSetIfChanged(ref _email, value);
	}

	public string Telefon
	{
		get => _telefon;
		set => this.RaiseAndSetIfChanged(ref _telefon, value);
	}

	public DateTimeOffset? DataPriema
	{
		get => _dataPriema;
		set => this.RaiseAndSetIfChanged(ref _dataPriema, value);
	}

	public string Status
	{
		get => _status;
		set => this.RaiseAndSetIfChanged(ref _status, value);
	}

	public bool IsValid =>
		!string.IsNullOrWhiteSpace(Familia) &&
		!string.IsNullOrWhiteSpace(Imya) &&
		!string.IsNullOrWhiteSpace(Dolzhnost);

	public void LoadFrom(EmployeeViewModel vm)
	{
		Familia        = vm.Familia;
		Imya           = vm.Imya;
		Otchestvo      = vm.Otchestvo;
		DataRozhdeniya = new DateTimeOffset(vm.DataRozhdeniya, TimeSpan.Zero);
		Dolzhnost      = vm.Dolzhnost;
		Email          = vm.Email;
		Telefon        = vm.Telefon;
		DataPriema     = new DateTimeOffset(vm.DataPriema, TimeSpan.Zero);
		Status         = vm.Status;
	}

	public void Reset()
	{
		Familia        = string.Empty;
		Imya           = string.Empty;
		Otchestvo      = string.Empty;
		DataRozhdeniya = DateTimeOffset.Now.AddYears(-30);
		Dolzhnost      = string.Empty;
		Email          = string.Empty;
		Telefon        = string.Empty;
		DataPriema     = DateTimeOffset.Now;
		Status         = "Активен";
	}
}

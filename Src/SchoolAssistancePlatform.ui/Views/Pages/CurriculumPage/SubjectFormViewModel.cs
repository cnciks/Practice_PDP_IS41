using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class SubjectFormViewModel : ReactiveObject
{
	private string _nazvanie     = string.Empty;
	private string _sokrashenie  = string.Empty;
	private int    _chasovNedelyu = 1;

	public string Nazvanie
	{
		get => _nazvanie;
		set => this.RaiseAndSetIfChanged(ref _nazvanie, value);
	}

	public string Sokrashenie
	{
		get => _sokrashenie;
		set => this.RaiseAndSetIfChanged(ref _sokrashenie, value);
	}

	public int ChasovNedelyu
	{
		get => _chasovNedelyu;
		set => this.RaiseAndSetIfChanged(ref _chasovNedelyu, value);
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(Nazvanie);

	public void LoadFrom(SubjectItemViewModel vm)
	{
		Nazvanie      = vm.Nazvanie;
		Sokrashenie   = vm.Sokrashenie;
		ChasovNedelyu = vm.ChasovNedelyu;
	}

	public void Reset()
	{
		Nazvanie      = string.Empty;
		Sokrashenie   = string.Empty;
		ChasovNedelyu = 1;
	}

	public UchebniyPredmetDto ToDto() => new()
	{
		Nazvanie      = Nazvanie,
		Sokrashenie   = Sokrashenie,
		ChasovNedelyu = ChasovNedelyu,
	};
}

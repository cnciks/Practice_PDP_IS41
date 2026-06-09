using System;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class PlanFormViewModel : ReactiveObject
{
	private string _nazvanie = string.Empty;
	private int _godNachala  = DateTime.Now.Year;
	private string _opisanie = string.Empty;

	public string Nazvanie
	{
		get => _nazvanie;
		set => this.RaiseAndSetIfChanged(ref _nazvanie, value);
	}

	public int GodNachala
	{
		get => _godNachala;
		set => this.RaiseAndSetIfChanged(ref _godNachala, value);
	}

	public string Opisanie
	{
		get => _opisanie;
		set => this.RaiseAndSetIfChanged(ref _opisanie, value);
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(Nazvanie);

	public void LoadFrom(PlanItemViewModel vm)
	{
		Nazvanie   = vm.Nazvanie;
		GodNachala = vm.GodNachala;
		Opisanie   = vm.Opisanie;
	}

	public void Reset()
	{
		Nazvanie   = string.Empty;
		GodNachala = DateTime.Now.Year;
		Opisanie   = string.Empty;
	}

	public UchebniyPlanDto ToDto() => new()
	{
		Nazvanie   = Nazvanie,
		GodNachala = GodNachala,
		Opisanie   = Opisanie,
	};
}

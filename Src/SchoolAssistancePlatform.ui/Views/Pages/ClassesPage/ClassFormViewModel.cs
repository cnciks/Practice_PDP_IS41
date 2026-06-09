using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

internal sealed class ClassFormViewModel : ReactiveObject
{
	private string         _nomerKlassa  = string.Empty;
	private int            _godObucheniya;
	private TeacherItem?   _selectedTeacher;
	private PlanItem?      _selectedPlan;

	public string NomerKlassa
	{
		get => _nomerKlassa;
		set => this.RaiseAndSetIfChanged(ref _nomerKlassa, value);
	}

	public int GodObucheniya
	{
		get => _godObucheniya;
		set => this.RaiseAndSetIfChanged(ref _godObucheniya, value);
	}

	public TeacherItem? SelectedTeacher
	{
		get => _selectedTeacher;
		set => this.RaiseAndSetIfChanged(ref _selectedTeacher, value);
	}

	public PlanItem? SelectedPlan
	{
		get => _selectedPlan;
		set => this.RaiseAndSetIfChanged(ref _selectedPlan, value);
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(NomerKlassa);

	public void LoadFrom(ClassItemViewModel vm, TeacherItem? teacher, PlanItem? plan)
	{
		NomerKlassa     = vm.Name;
		GodObucheniya   = vm.GodObucheniya;
		SelectedTeacher = teacher;
		SelectedPlan    = plan;
	}

	public void Reset(int currentYear)
	{
		NomerKlassa     = string.Empty;
		GodObucheniya   = currentYear;
		SelectedTeacher = null;
		SelectedPlan    = null;
	}

	public KlassDto ToDto() => new()
	{
		NomerKlassa          = NomerKlassa,
		GodObucheniya        = GodObucheniya,
		KlassRukovoditelID   = SelectedTeacher?.SotrudnikID ?? 0,
		PlanID               = SelectedPlan?.PlanID ?? 0,
	};
}

internal sealed class TeacherItem
{
	public long   SotrudnikID { get; init; }
	public string FullName    { get; init; } = string.Empty;

	public override string ToString() => FullName;

	public static TeacherItem FromDto(SotrudnikDto dto) => new()
	{
		SotrudnikID = dto.SotrudnikID,
		FullName    = string.IsNullOrWhiteSpace(dto.Otchestvo)
			? $"{dto.Familia} {dto.Imya}"
			: $"{dto.Familia} {dto.Imya} {dto.Otchestvo}",
	};
}

internal sealed class PlanItem
{
	public long   PlanID   { get; init; }
	public string Nazvanie { get; init; } = string.Empty;

	public override string ToString() => Nazvanie;

	public static PlanItem FromDto(UchebniyPlanDto dto) => new()
	{
		PlanID   = dto.PlanID,
		Nazvanie = dto.Nazvanie ?? string.Empty,
	};
}

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal sealed class ScheduleFormViewModel : ReactiveObject
{
	private KlassItem?   _selectedKlass;
	private SubjectItem? _selectedSubject;
	private TeacherItem? _selectedTeacher;
	private int          _denNedeli = 1;
	private int          _nomerUroka = 1;
	private string       _kabinet    = string.Empty;

	public KlassItem? SelectedKlass
	{
		get => _selectedKlass;
		set => this.RaiseAndSetIfChanged(ref _selectedKlass, value);
	}

	public SubjectItem? SelectedSubject
	{
		get => _selectedSubject;
		set => this.RaiseAndSetIfChanged(ref _selectedSubject, value);
	}

	public TeacherItem? SelectedTeacher
	{
		get => _selectedTeacher;
		set => this.RaiseAndSetIfChanged(ref _selectedTeacher, value);
	}

	public int DenNedeli
	{
		get => _denNedeli;
		set => this.RaiseAndSetIfChanged(ref _denNedeli, value);
	}

	public int NomerUroka
	{
		get => _nomerUroka;
		set => this.RaiseAndSetIfChanged(ref _nomerUroka, value);
	}

	public string Kabinet
	{
		get => _kabinet;
		set => this.RaiseAndSetIfChanged(ref _kabinet, value);
	}

	public bool IsValid =>
		SelectedKlass   is not null &&
		SelectedSubject is not null &&
		SelectedTeacher is not null;

	public void LoadFrom(LessonCellViewModel cell, KlassItem? klass, SubjectItem? subject, TeacherItem? teacher)
	{
		SelectedKlass   = klass;
		SelectedSubject = subject;
		SelectedTeacher = teacher;
		DenNedeli       = (int)cell.DenNedeli;
		NomerUroka      = cell.NomerUroka;
		Kabinet         = cell.Room ?? string.Empty;
	}

	public void Reset(int defaultDay = 1, int defaultLesson = 1)
	{
		SelectedKlass   = null;
		SelectedSubject = null;
		SelectedTeacher = null;
		DenNedeli       = defaultDay;
		NomerUroka      = defaultLesson;
		Kabinet         = string.Empty;
	}

	public RaspisanieDto ToDto() => new()
	{
		KlassID    = SelectedKlass?.KlassID ?? 0,
		PredmetID  = SelectedSubject?.PredmetID ?? 0,
		SotrudnikID = SelectedTeacher?.SotrudnikID ?? 0,
		DenNedeli  = DenNedeli,
		NomerUroka = (short)NomerUroka,
		Kabinet    = Kabinet,
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

internal sealed class SubjectItem
{
	public long   PredmetID { get; init; }
	public string Nazvanie  { get; init; } = string.Empty;

	public override string ToString() => Nazvanie;

	public static SubjectItem FromDto(UchebniyPredmetDto dto) => new()
	{
		PredmetID = dto.PredmetID,
		Nazvanie  = dto.Nazvanie ?? string.Empty,
	};
}

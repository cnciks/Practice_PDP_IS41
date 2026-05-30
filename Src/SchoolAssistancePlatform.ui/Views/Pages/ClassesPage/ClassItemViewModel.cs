using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

public class ClassItemViewModel : ReactiveObject
{
	private readonly ClassService _classService;

	private KlassDto _class;
	private string _name;
	private int _studentsCount;
	private string _teacher;
	private int _year;

	public long Id => _class?.KlassID ?? 0;
	public long TeacherId => _class?.KlassRukovoditelID ?? 0;
	public long PlanId => _class?.PlanID ?? 0;

	public string Name
	{
		get => _name;
		set => this.RaiseAndSetIfChanged(ref _name, value);
	}

	public int StudentsCount
	{
		get => _studentsCount;
		set => this.RaiseAndSetIfChanged(ref _studentsCount, value);
	}

	public string Teacher
	{
		get => _teacher;
		set => this.RaiseAndSetIfChanged(ref _teacher, value);
	}

	public int Year
	{
		get => _year;
		set => this.RaiseAndSetIfChanged(ref _year, value);
	}

	public ClassItemViewModel(KlassDto klass, int studentsCount, string teacherName, ClassService classService)
	{
		_class = klass;
		_classService = classService;

		Name = klass.NomerKlassa;
		Year = klass.GodObucheniya;
		StudentsCount = studentsCount;
		Teacher = teacherName;
	}
}

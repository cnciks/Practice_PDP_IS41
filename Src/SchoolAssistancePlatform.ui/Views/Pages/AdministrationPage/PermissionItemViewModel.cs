using ReactiveUI;

using SchoolAssistancePlatform.framework;

namespace SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;

internal sealed class PermissionItemViewModel : ReactiveObject
{
	private bool _isChecked;

	public Permissions Permission { get; init; }
	public string      Label      { get; init; } = string.Empty;

	public bool IsChecked
	{
		get => _isChecked;
		set => this.RaiseAndSetIfChanged(ref _isChecked, value);
	}

	public static PermissionItemViewModel FromPermission(Permissions p, bool isChecked = false) => new()
	{
		Permission = p,
		Label      = p switch
		{
			Permissions.MainPage           => "Главная страница",
			Permissions.AdministrationPage => "Администрирование",
			Permissions.FinancePage        => "Финансы",
			Permissions.CurriculumPage     => "Учебный план",
			Permissions.SchedulePage       => "Расписание",
			Permissions.JournalPage        => "Журнал успеваемости",
			Permissions.EmployeesPage      => "Сотрудники",
			Permissions.StudentsPage       => "Ученики",
			Permissions.MessagesPage       => "Сообщения",
			Permissions.CommandsPage       => "Приказы",
			Permissions.ClassesPage        => "Классы",
			Permissions.ReportsPage        => "Отчёты",
			_                              => p.ToString(),
		},
		IsChecked = isChecked,
	};
}

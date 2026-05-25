namespace SchoolAssistancePlatform.framework;

/// <summary>
/// Перечисление доступных страниц/разделов системы.
/// Используется для определения прав доступа ролей.
/// </summary>
[Flags]
public enum Permissions : long
{
	/// <summary> Нет доступа. </summary>
	None = 0,


	MainPage,
	AdministrationPage,
	FinancePage,

	CurriculumPage,
	SchedulePage,
	JournalPage,


	EmployeesPage,
	StudentsPage,


	MessagesPage,
	CommandsPage,

	ClassesPage,

	ReportsPage
}

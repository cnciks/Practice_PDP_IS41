using System.Collections.Generic;
using System.Linq;

using ReactiveUI;

using SchoolAssistancePlatform.framework;

namespace SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;

internal sealed class UserViewModel : ReactiveObject
{
	public long    Id          { get; init; }
	public string  Login       { get; init; } = string.Empty;
	public string  Email       { get; init; } = string.Empty;
	public string  RoleName    { get; init; } = string.Empty;
	public long    RoleId      { get; init; }
	public string? SotrudnikFIO { get; init; }

	public bool   HasSotrudnik => !string.IsNullOrWhiteSpace(SotrudnikFIO);
	public string SotrudnikText => HasSotrudnik ? SotrudnikFIO! : "—";

	public string Initials  => Login.Length >= 2
		? Login[..2].ToUpper()
		: Login.ToUpper();

	public string AvatarColor => ((Login.GetHashCode() & 0x7FFFFFFF) % 6) switch
	{
		0 => "#3498DB",
		1 => "#9B59B6",
		2 => "#27AE60",
		3 => "#E67E22",
		4 => "#E74C3C",
		_ => "#16A085",
	};
}

internal sealed class RoleViewModel : ReactiveObject
{
	public long                   Id          { get; init; }
	public string                 Name        { get; init; } = string.Empty;
	public string                 Description { get; init; } = string.Empty;
	public List<Permissions>      Permissions { get; init; } = [];

	public int    UserCount  { get; set; }
	public string PermText   => Permissions.Count == 0 ? "нет прав"
		: string.Join(", ", Permissions.Select(PermissionName));

	private static string PermissionName(Permissions p) => p switch
	{
		framework.Permissions.MainPage           => "Главная",
		framework.Permissions.AdministrationPage => "Администрирование",
		framework.Permissions.FinancePage        => "Финансы",
		framework.Permissions.CurriculumPage     => "Учебный план",
		framework.Permissions.SchedulePage       => "Расписание",
		framework.Permissions.JournalPage        => "Журнал",
		framework.Permissions.EmployeesPage      => "Сотрудники",
		framework.Permissions.StudentsPage       => "Ученики",
		framework.Permissions.MessagesPage       => "Сообщения",
		framework.Permissions.CommandsPage       => "Приказы",
		framework.Permissions.ClassesPage        => "Классы",
		framework.Permissions.ReportsPage        => "Отчёты",
		_                                        => p.ToString(),
	};
}

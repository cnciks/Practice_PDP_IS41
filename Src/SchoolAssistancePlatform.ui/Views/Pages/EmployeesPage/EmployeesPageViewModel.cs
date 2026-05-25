using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal class EmployeesPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Сотрудники";

	public Permissions Permission => Permissions.EmployeesPage;
}

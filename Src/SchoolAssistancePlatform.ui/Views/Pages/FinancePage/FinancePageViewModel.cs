using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.FinancePage;

internal class FinancePageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Финансы";

	public Permissions Permission => Permissions.EmployeesPage;
}

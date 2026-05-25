using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.ReportsPage;

internal class ReportsPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Отчёты";

	public Permissions Permission => Permissions.ReportsPage;
}

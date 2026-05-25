using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

internal class StudentsPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Ученики";

	public Permissions Permission => Permissions.StudentsPage;
}

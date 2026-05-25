using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

internal class ClassesPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Классы";

	public Permissions Permission => Permissions.ClassesPage;
}

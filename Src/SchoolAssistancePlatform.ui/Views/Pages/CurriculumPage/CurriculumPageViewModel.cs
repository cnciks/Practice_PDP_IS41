using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal class CurriculumPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Учебный план";

	public Permissions Permission => Permissions.CurriculumPage;
}

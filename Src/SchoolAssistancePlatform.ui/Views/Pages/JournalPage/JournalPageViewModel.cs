using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal class JournalPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Журнал";

	public Permissions Permission => Permissions.JournalPage;
}

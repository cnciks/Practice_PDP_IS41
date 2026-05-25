using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.MessagesPage;

internal class MessagesPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Сообщения";

	public Permissions Permission => Permissions.MessagesPage;
}

using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.CommandsPage;

internal class CommandsPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Приказы";

	public Permissions Permission => Permissions.CommandsPage;

	public Task LoadPageAsync()
	{
		return Task.CompletedTask;
	}
}

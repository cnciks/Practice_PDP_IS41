using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;

internal class AdministrationPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Администрирование";

	public Permissions Permission => Permissions.AdministrationPage;

	public Task LoadPageAsync()
	{
		return Task.CompletedTask;
	}
}

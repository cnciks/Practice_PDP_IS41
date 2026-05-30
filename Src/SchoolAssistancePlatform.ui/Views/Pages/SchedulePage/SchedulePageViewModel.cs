using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal class SchedulePageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Расписание";

	public Permissions Permission => Permissions.SchedulePage;

	#region Data

	#endregion

	#region Property

	public ReactiveCommand<Unit, Task> EditScheduleCommand { get; }


	#endregion

	#region .ctor

	public SchedulePageViewModel()
	{
		EditScheduleCommand = ReactiveCommand.Create(EditSchedule);
	}

	private Task EditSchedule()
	{
		return Task.CompletedTask;
	}

	public Task LoadPageAsync()
	{
		return Task.CompletedTask;
	}

	#endregion

}

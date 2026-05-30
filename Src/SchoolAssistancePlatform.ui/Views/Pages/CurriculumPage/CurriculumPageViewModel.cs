using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal class CurriculumPageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Учебный план";

	public Permissions Permission => Permissions.CurriculumPage;

	#region Data

	#endregion

	#region Property

	public ReactiveCommand<Unit, Task> EditCurriculumCommand { get; }

	#endregion

	#region .ctor

	public CurriculumPageViewModel()
	{
		EditCurriculumCommand = ReactiveCommand.Create(EditCurriculum);
	}

	private Task EditCurriculum()
	{
		return Task.CompletedTask;
	}

	public Task LoadPageAsync()
	{
		return Task.CompletedTask;
	}

	#endregion
}

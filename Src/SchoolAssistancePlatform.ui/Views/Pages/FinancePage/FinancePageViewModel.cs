using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.FinancePage;

internal class FinancePageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Финансы";

	public Permissions Permission => Permissions.EmployeesPage;

	#region Data

	#endregion

	#region Property

	public ReactiveCommand<Unit, Task> ExportCommand { get; }

	public ReactiveCommand<Unit, Task> ReportCommand { get; }

	#endregion

	#region .ctor

	public FinancePageViewModel()
	{
		ExportCommand = ReactiveCommand.Create(Export);

		ReportCommand = ReactiveCommand.Create(Report);
	}

	private Task Report()
	{
		return Task.CompletedTask;
	}

	private Task Export()
	{
		return Task.CompletedTask;
	}

	public Task LoadPageAsync()
	{
		return Task.CompletedTask;
	}

	#endregion

}

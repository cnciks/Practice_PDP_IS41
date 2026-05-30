using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal class JournalPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly GradeJournalService _gradeJournalService;

	#endregion

	#region Property
	public string Title => "Журнал";

	public Permissions Permission => Permissions.JournalPage;

	public AvaloniaList<StudentGradeViewModel> Students { get; } = [];

	public AvaloniaList<DateTime> Dates { get; } = [];

	public ReactiveCommand<Unit, Task> SaveJournalCommand { get; }

	#endregion

	#region .ctor

	public JournalPageViewModel(GradeJournalService gradeJournalService)
	{
		_gradeJournalService = gradeJournalService;

		SaveJournalCommand = ReactiveCommand.Create(SaveJournal);
	}

	#endregion

	private Task SaveJournal()
	{
		return Task.CompletedTask;
	}

	public async Task LoadPageAsync()
	{
		await LoadJournal();
	}

	private async Task LoadJournal()
	{
		try
		{
			var _selectedKlassID = 317;

			var startDate = DateTime.MinValue;
			var endDate = DateTime.Now.AddMonths(1).AddDays(-1);

			var dates = await _gradeJournalService.GetLessonDatesAsync(_selectedKlassID, startDate, endDate);

			Dates.Clear();
			Dates.AddRange(dates);

			var students = await _gradeJournalService.GetStudentsWithGradesAsync(_selectedKlassID, startDate, endDate);

			var items = new List<StudentGradeViewModel>();
			foreach(var student in students)
			{
				items.Add(new StudentGradeViewModel(student, _gradeJournalService));
			}

			Students.Clear();
			Students.AddRange(items);
		}
		catch(Exception ex)
		{

		}
	}

}

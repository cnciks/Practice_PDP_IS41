using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

internal class StudentsPageViewModel : ReactiveObject, IWorkSpacePage
{
	#region Data

	private readonly StudentService _studentService;

	#endregion

	#region Property

	public string Title => "Ученики";

	public Permissions Permission => Permissions.StudentsPage;

	public AvaloniaList<StudentViewModel> Students { get; } = [];

	public ReactiveCommand<Unit, Task> AddStudentsCommand { get; }

	#endregion

	#region .ctor

	public StudentsPageViewModel(StudentService studentService)
	{
		_studentService = studentService;

		AddStudentsCommand = ReactiveCommand.Create(AddStudents);
	}

	private Task AddStudents()
	{
		return Task.CompletedTask;
	}

	public async Task LoadPageAsync()
	{
		await LoadStudents();
	}

	private async Task LoadStudents()
	{
		try
		{
			var students = await _studentService.GetAllStudentsAsync();

			var items = new List<StudentViewModel>();

			foreach(var student in students)
			{
				var averageScore = await _studentService.GetAverageScoreAsync(student.UchenikID);
				var fullName     = await _studentService.GetStudentFullNameAsync(student.UchenikID);
				var className    = await _studentService.GetStudentClassAsync(student.UchenikID);
				var parents      = await _studentService.GetStudentParentsAsync(student.UchenikID);

				items.Add(new StudentViewModel(student, averageScore, fullName, className, parents, _studentService));
			}

			Students.Clear();

			Students.AddRange(items);
		}
		catch(Exception ex)
		{

		}
	}

	#endregion
}

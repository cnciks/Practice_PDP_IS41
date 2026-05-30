using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;

internal class ClassesPageViewModel : ReactiveObject, IWorkSpacePage
{

	#region Data

	private readonly ClassService _classService;

	#endregion

	#region Property
	public string Title => "Классы";

	public Permissions Permission => Permissions.ClassesPage;

	public AvaloniaList<ClassItemViewModel> Classes { get; } = [];

	public ReactiveCommand<Unit, Task> AddClassesCommand { get; }

	public ReactiveCommand<Unit, Task> EditClassCommand { get; }

	#endregion

	#region .ctor

	public ClassesPageViewModel(ClassService classService)
	{
		_classService = classService;

		AddClassesCommand = ReactiveCommand.Create(AddClasses);

		EditClassCommand = ReactiveCommand.Create(EditClass);
	}

	private Task EditClass()
	{
		return Task.CompletedTask;
	}

	#endregion

	private Task AddClasses()
	{
		return Task.CompletedTask;
	}

	public async Task LoadPageAsync()
	{
		await LoadClasses();
	}

	private async Task LoadClasses()
	{
		try
		{
			var classes = await _classService.GetAllClassesAsync();

			var items = new List<ClassItemViewModel>();
			foreach(var klass in classes)
			{
				var studentsCount = await _classService.GetStudentsCountAsync(klass.KlassID);
				var teacherName = await _classService.GetTeacherNameAsync(klass.KlassRukovoditelID);

				items.Add(new ClassItemViewModel(klass, studentsCount, teacherName, _classService));
			}
			Classes.Clear();

			Classes.AddRange(items);
		}
		catch(Exception ex)
		{

		}
	}
}

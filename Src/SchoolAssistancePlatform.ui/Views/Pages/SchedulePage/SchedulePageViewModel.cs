using Avalonia.Collections;

using ReactiveUI;

using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;

internal class SchedulePageViewModel : ReactiveObject, IWorkSpacePage
{
	public string Title => "Расписание";

	public Permissions Permission => Permissions.SchedulePage;

	private AvaloniaList<string> _classes = new() { "5А", "5Б", "5В" }; // Классы
	private AvaloniaList<Lesson> _lessons; // Расписание уроков

	public AvaloniaList<string> Classes => _classes;
	public AvaloniaList<Lesson> Lessons => _lessons ?? (_lessons = CreateLessons());

	// Метод формирования расписания уроков
	private AvaloniaList<Lesson> CreateLessons()
	{
		return new AvaloniaList<Lesson>()
			{
				new Lesson("8:30", "Математика", "Петрова"),
				new Lesson("9:20", "Русский язык", "Смирнова"),
				new Lesson("10:10", "Английский язык", "Кларк")
			};
	}

	public class Lesson
	{
		public string Time { get; }
		public string Subject { get; }
		public string Teacher { get; }

		public Lesson(string time, string subject, string teacher)
		{
			Time = time;
			Subject = subject;
			Teacher = teacher;
		}
	}
}

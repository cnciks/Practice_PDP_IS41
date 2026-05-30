using System.Collections.Generic;

using ReactiveUI;

using SchoolAssistancePlatform.UI.Data;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal class StudentGradeViewModel : ReactiveObject
{
	private readonly GradeJournalService _gradeJournalService;
	private StudentGradeDto _student;
	private string _studentName;
	private List<int?> _grades;
	private double _averageGrade;
	private bool _isEditing;

	public long StudentID => _student?.StudentID ?? 0;

	public string StudentName
	{
		get => _studentName;
		set => this.RaiseAndSetIfChanged(ref _studentName, value);
	}

	public List<int?> Grades
	{
		get => _grades;
		set => this.RaiseAndSetIfChanged(ref _grades, value);
	}

	public double AverageGrade
	{
		get => _averageGrade;
		set => this.RaiseAndSetIfChanged(ref _averageGrade, value);
	}

	public StudentGradeViewModel(StudentGradeDto student, GradeJournalService gradeJournalService)
	{
		_student = student;
		_gradeJournalService = gradeJournalService;

		StudentName  = student.StudentName;
		Grades       = student.Grades ?? new List<int?>();
		AverageGrade = student.AverageGrade;
	}
}

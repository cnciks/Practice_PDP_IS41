using System.Collections.Generic;

using ReactiveUI;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal sealed class StudentRowViewModel : ReactiveObject
{
	private double _averageGrade;

	public long   StudentID   { get; init; }
	public string StudentName { get; init; } = string.Empty;

	public List<GradeCellViewModel> Grades { get; init; } = [];

	public double AverageGrade
	{
		get => _averageGrade;
		set => this.RaiseAndSetIfChanged(ref _averageGrade, value);
	}

	public string AverageColor => AverageGrade switch
	{
		>= 4.5 => "#27AE60",
		>= 3.5 => "#3498DB",
		>= 2.5 => "#F39C12",
		> 0    => "#E74C3C",
		_      => "#95A5A6",
	};
}

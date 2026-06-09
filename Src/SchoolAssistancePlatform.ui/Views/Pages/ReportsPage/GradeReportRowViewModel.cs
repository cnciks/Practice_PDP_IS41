namespace SchoolAssistancePlatform.UI.Views.Pages.ReportsPage;

internal sealed class GradeReportRowViewModel
{
	public string Klass        { get; init; } = string.Empty;
	public int    Students     { get; init; }
	public double AvgGrade     { get; init; }
	public int    Excellent    { get; init; }
	public int    Good         { get; init; }
	public int    Satisfactory { get; init; }
	public int    Poor         { get; init; }

	public string AvgGradeText    => AvgGrade > 0 ? AvgGrade.ToString("F2") : "—";
	public string AvgGradeColor   => AvgGrade switch
	{
		>= 4.5 => "#27AE60",
		>= 3.5 => "#3498DB",
		>= 2.5 => "#E67E22",
		> 0    => "#E74C3C",
		_      => "#AAAAAA",
	};
}

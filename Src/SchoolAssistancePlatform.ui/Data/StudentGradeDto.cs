using System.Collections.Generic;

namespace SchoolAssistancePlatform.UI.Data;

public class StudentGradeDto
{
	public long StudentID { get; set; }

	public string StudentName { get; set; }

	public List<int?> Grades { get; set; }

	public double AverageGrade { get; set; }

	public bool IsEditing { get; set; }
}

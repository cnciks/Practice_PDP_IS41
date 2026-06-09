using System;
using System.Collections.Generic;

using SchoolAssistancePlatform.Base.Entity.School;

namespace SchoolAssistancePlatform.UI.Data;

public class StudentJournalDto
{
	public long StudentID { get; set; }

	public string StudentName { get; set; } = string.Empty;

	public double AverageGrade { get; set; }

	/// <summary>
	/// Ключ — дата урока (только дата, без времени).
	/// Значение — запись журнала (первая по ZapisID в этот день).
	/// </summary>
	public Dictionary<DateTime, ZhurnalUspevaemostiEntity> GradesByDate { get; set; } = [];
}

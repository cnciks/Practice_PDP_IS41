using System;
using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

public class StudentViewModel : ReactiveObject
{
	private readonly StudentService _studentService;

	private UchenikDto _student;
	private string _fullName;
	private string _class;
	private string _birthDate;
	private string _parents;
	private string _averageScore;
	private double _averageScoreValue;

	public long Id => _student?.UchenikID ?? 0;

	public string FullName
	{
		get => _fullName;
		set => this.RaiseAndSetIfChanged(ref _fullName, value);
	}

	public string Class
	{
		get => _class;
		set => this.RaiseAndSetIfChanged(ref _class, value);
	}

	public string BirthDate
	{
		get => _birthDate;
		set => this.RaiseAndSetIfChanged(ref _birthDate, value);
	}

	public string Parents
	{
		get => _parents;
		set => this.RaiseAndSetIfChanged(ref _parents, value);
	}

	public string AverageScore
	{
		get => _averageScore;
		set => this.RaiseAndSetIfChanged(ref _averageScore, value);
	}

	public ReactiveCommand<Unit, Unit> EditCommand { get; }

	public StudentViewModel(
		UchenikDto student,
		double averageScore,
		string fullName,
		string className,
		string parents,
		StudentService studentService)
	{
		_student = student;
		_studentService = studentService;
		_averageScoreValue = averageScore;

		UpdateProperties(fullName, className, parents);

		EditCommand = ReactiveCommand.CreateFromTask(Edit);
	}

	private void UpdateProperties(string fullName, string className, string parents)
	{
		FullName = fullName;
		Class = className;
		BirthDate = _student.DataRozhdeniya.ToString("dd.MM.yyyy");
		Parents = parents;
		AverageScore = _averageScoreValue > 0 ? _averageScoreValue.ToString("F2") : "Нет оценок";
	}

	private async Task UpdateAverageScore()
	{
		_averageScoreValue = await _studentService.GetAverageScoreAsync(Id);
		AverageScore = _averageScoreValue > 0 ? _averageScoreValue.ToString("F2") : "Нет оценок";
	}

	private async Task Edit()
	{
		try
		{
			var updateDto = new UchenikDto
			{
				Familiia          = _student.Familiia,
				Imya              = _student.Imya,
				Otchestvo         = _student.Otchestvo,
				DataRozhdeniya    = _student.DataRozhdeniya,
				AdresProzhivaniya = _student.AdresProzhivaniya,
				FIORoditeley      = _student.FIORoditeley,
				TelefonRoditelya  = _student.TelefonRoditelya,
				DataZachisleniya  = _student.DataZachisleniya,
				KlassID           = _student.KlassID
			};
		}
		catch(Exception ex)
		{

		}
	}
}

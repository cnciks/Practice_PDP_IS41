using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;

internal sealed class StudentViewModel : ReactiveObject
{
	private string _averageScore = string.Empty;

	public long   UchenikID { get; init; }
	public string Familiia  { get; init; } = string.Empty;
	public string Imya      { get; init; } = string.Empty;
	public string Otchestvo { get; init; } = string.Empty;
	public long   KlassID   { get; init; }

	public string FullName  { get; init; } = string.Empty;
	public string Class     { get; init; } = string.Empty;
	public string BirthDate { get; init; } = string.Empty;
	public string Parents   { get; init; } = string.Empty;

	public string AverageScore
	{
		get => _averageScore;
		set => this.RaiseAndSetIfChanged(ref _averageScore, value);
	}

	public string Initials => Familiia.Length > 0 && Imya.Length > 0
		? $"{Familiia[0]}{Imya[0]}"
		: "??";

	public string AvatarColor => ((Familiia.GetHashCode() & 0x7FFFFFFF) % 6) switch
	{
		0 => "#3498DB",
		1 => "#9B59B6",
		2 => "#27AE60",
		3 => "#E67E22",
		4 => "#E74C3C",
		_ => "#16A085",
	};

	public string ScoreBadgeColor => double.TryParse(
		AverageScore.Replace(',', '.'),
		System.Globalization.NumberStyles.Any,
		System.Globalization.CultureInfo.InvariantCulture, out var v)
		? v switch
		{
			>= 4.5 => "#27AE60",
			>= 3.5 => "#3498DB",
			>= 2.5 => "#E67E22",
			> 0    => "#E74C3C",
			_      => "#95A5A6",
		}
		: "#95A5A6";

	public static StudentViewModel FromDto(
		UchenikDto dto,
		double avgScore,
		string fullName,
		string className,
		string parents) => new()
	{
		UchenikID    = dto.UchenikID,
		Familiia     = dto.Familiia     ?? string.Empty,
		Imya         = dto.Imya         ?? string.Empty,
		Otchestvo    = dto.Otchestvo    ?? string.Empty,
		KlassID      = dto.KlassID,
		FullName     = fullName,
		Class        = className,
		BirthDate    = dto.DataRozhdeniya.ToString("dd.MM.yyyy"),
		Parents      = parents,
		AverageScore = avgScore > 0 ? avgScore.ToString("F2") : "—",
	};
}

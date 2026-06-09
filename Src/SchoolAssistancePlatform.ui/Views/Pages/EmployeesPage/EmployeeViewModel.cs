using System;

using ReactiveUI;

using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;

internal sealed class EmployeeViewModel : ReactiveObject
{
	public long     SotrudnikID     { get; init; }
	public string   Familia         { get; init; } = string.Empty;
	public string   Imya            { get; init; } = string.Empty;
	public string   Otchestvo       { get; init; } = string.Empty;
	public DateTime DataRozhdeniya  { get; init; }
	public string   Dolzhnost       { get; init; } = string.Empty;
	public string   Email           { get; init; } = string.Empty;
	public string   Telefon         { get; init; } = string.Empty;
	public DateTime DataPriema      { get; init; }
	public string   Status          { get; init; } = string.Empty;
	public string   ClassLeadership { get; init; } = string.Empty;
	public string   RoomNumber      { get; init; } = string.Empty;

	public string FullName => string.IsNullOrWhiteSpace(Otchestvo)
		? $"{Familia} {Imya}"
		: $"{Familia} {Imya} {Otchestvo}";

	public string Initials => Familia.Length > 0 && Imya.Length > 0
		? $"{Familia[0]}{Imya[0]}"
		: "??";

	public string AvatarColor => ((Familia.GetHashCode() & 0x7FFFFFFF) % 6) switch
	{
		0 => "#3498DB",
		1 => "#9B59B6",
		2 => "#27AE60",
		3 => "#E67E22",
		4 => "#E74C3C",
		_ => "#16A085",
	};

	public string StatusColor => Status switch
	{
		"Активен"  or "Работает"  => "#27AE60",
		"Отпуск"   or "Больничный" => "#E67E22",
		"Уволен"                   => "#E74C3C",
		_                          => "#7F8C8D",
	};

	public static EmployeeViewModel FromDto(SotrudnikDto dto, string classLeadership, string roomNumber) => new()
	{
		SotrudnikID    = dto.SotrudnikID,
		Familia        = dto.Familia        ?? string.Empty,
		Imya           = dto.Imya           ?? string.Empty,
		Otchestvo      = dto.Otchestvo      ?? string.Empty,
		DataRozhdeniya = dto.DataRozhdeniya,
		Dolzhnost      = dto.Dolzhnost      ?? string.Empty,
		Email          = dto.Email          ?? string.Empty,
		Telefon        = dto.Telefon        ?? string.Empty,
		DataPriema     = dto.DataPriema,
		Status         = dto.Status         ?? string.Empty,
		ClassLeadership = classLeadership,
		RoomNumber      = roomNumber,
	};
}

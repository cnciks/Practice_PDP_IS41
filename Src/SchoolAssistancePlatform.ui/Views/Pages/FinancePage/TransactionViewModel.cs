using System;

namespace SchoolAssistancePlatform.UI.Views.Pages.FinancePage;

internal sealed class TransactionViewModel
{
	public long PlatezhID { get; init; }

	public string Student { get; init; } = string.Empty;

	public string Klass { get; init; } = string.Empty;

	public DateTime Date { get; init; }

	public string DateText => Date.ToString("dd.MM.yyyy");

	public string Naznachenie { get; init; } = string.Empty;

	public string TipOperacii { get; init; } = string.Empty;

	public decimal Summa { get; init; }

	public bool IsIncome => TipOperacii is "Приход" or "Оплата";

	public string AmountText => $"{(IsIncome ? "+" : "−")}{Summa:N2} ₽";

	public string AmountColor => IsIncome ? "#27AE60" : "#E74C3C";

	public string BadgeColor => TipOperacii switch
	{
		"Приход" => "#27AE60",
		"Оплата" => "#3498DB",
		"Расход" => "#E74C3C",
		"Возврат" => "#F39C12",
		_ => "#95A5A6",
	};
}

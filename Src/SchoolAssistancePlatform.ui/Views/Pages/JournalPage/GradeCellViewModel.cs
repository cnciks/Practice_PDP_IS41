using System;

using ReactiveUI;

namespace SchoolAssistancePlatform.UI.Views.Pages.JournalPage;

internal sealed class GradeCellViewModel : ReactiveObject
{
	private string _value = string.Empty;

	public long     StudentID    { get; init; }
	public long     RaspisanieID { get; init; }
	public DateTime Date         { get; init; }

	/// <summary>
	/// Текстовое значение оценки (1-5 или пусто). Изменение помечает ячейку как изменённую.
	/// </summary>
	public string Value
	{
		get => _value;
		set
		{
			this.RaiseAndSetIfChanged(ref _value, value);
			IsDirty = true;
		}
	}

	public bool IsDirty { get; private set; }

	public string ForegroundColor => Value switch
	{
		"5" => "#27AE60",
		"4" => "#3498DB",
		"3" => "#F39C12",
		"2" => "#E74C3C",
		_   => "#555555",
	};

	public void MarkClean() => IsDirty = false;
}

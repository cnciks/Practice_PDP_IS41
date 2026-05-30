using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет учебный предмет (дисциплину)
/// </summary>
[Table("UchebniyPredmet", Schema = "School")]
public class UchebniyPredmetEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long PredmetID { get; set; }

	/// <summary>
	/// Название предмета
	/// </summary>
	[Required(ErrorMessage = "Название обязательно"), StringLength(255)]
	public string Nazvanie { get; set; }

	/// <summary>
	/// Сокращение (аббревиатура)
	/// </summary>
	[Required(ErrorMessage = "Сокращение обязательно"), StringLength(10)]
	public string Sokrashenie { get; set; }

	/// <summary>
	/// Количество часов в неделю
	/// </summary>
	[Required(ErrorMessage = "Количество часов обязательно")]
	public int ChasovNedelyu { get; set; }
}

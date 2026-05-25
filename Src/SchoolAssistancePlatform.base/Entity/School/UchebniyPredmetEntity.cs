using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("UchebniyPredmet", Schema = "School")]
public class UchebniyPredmetEntity
{
	// Первичный ключ
	[Key]
	public long PredmetID { get; set; }

	// Название предмета
	[Required(ErrorMessage = "Название обязательно"), StringLength(255)]
	public string Nazvanie { get; set; }

	// Сокращение (аббревиатура)
	[Required(ErrorMessage = "Сокращение обязательно"), StringLength(10)]
	public string Sokrashenie { get; set; }

	// Количество часов в неделю
	[Required(ErrorMessage = "Количество часов обязательно")]
	public int ChasovNedelyu { get; set; }
}

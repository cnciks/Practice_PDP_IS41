using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Raspisanie", Schema = "School")]
public class RaspisanieEntity
{
	// Первичный ключ
	[Key]
	public long RaspisanieID { get; set; }

	// Внешний ключ, ссылка на класс
	[Required(ErrorMessage = "Класс обязателен")]
	public long KlassID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("KlassID")]
	public virtual KlassEntity Klass { get; set; }

	// Внешний ключ, ссылка на предмет
	[Required(ErrorMessage = "Предмет обязателен")]
	public long PredmetID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("PredmetID")]
	public virtual UchebniyPredmetEntity UchebniyPredmet { get; set; }

	// Внешний ключ, ссылка на преподавателя
	[Required(ErrorMessage = "Преподаватель обязателен")]
	public long SotrudnikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("SotrudnikID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }

	// День недели (1-понедельник, ..., 7-воскресенье)
	[Required(ErrorMessage = "День недели обязателен")]
	public long DenNedeli { get; set; }

	// Номер урока в расписании
	[Required(ErrorMessage = "Номер урока обязателен")]
	public short NomerUroka { get; set; }

	// Кабинет проведения занятия
	[Required(ErrorMessage = "Кабинет обязателен"), StringLength(10)]
	public string Kabinet { get; set; }
}

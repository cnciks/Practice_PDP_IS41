using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет расписание занятий (уроков) для класса
/// </summary>
[Table("Raspisanie", Schema = "School")]
public class RaspisanieEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long RaspisanieID { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на класс
	/// </summary>
	[Required(ErrorMessage = "Класс обязателен")]
	public long KlassID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("KlassID")]
	public virtual KlassEntity Klass { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на предмет
	/// </summary>
	[Required(ErrorMessage = "Предмет обязателен")]
	public long PredmetID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("PredmetID")]
	public virtual UchebniyPredmetEntity UchebniyPredmet { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на преподавателя
	/// </summary>
	[Required(ErrorMessage = "Преподаватель обязателен")]
	public long SotrudnikID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("SotrudnikID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }

	/// <summary>
	/// День недели (1-понедельник, ..., 7-воскресенье)
	/// </summary>
	[Required(ErrorMessage = "День недели обязателен")]
	public long DenNedeli { get; set; }

	/// <summary>
	/// Номер урока в расписании
	/// </summary>
	[Required(ErrorMessage = "Номер урока обязателен")]
	public short NomerUroka { get; set; }

	/// <summary>
	/// Кабинет проведения занятия
	/// </summary>
	[Required(ErrorMessage = "Кабинет обязателен"), StringLength(10)]
	public string Kabinet { get; set; }
}

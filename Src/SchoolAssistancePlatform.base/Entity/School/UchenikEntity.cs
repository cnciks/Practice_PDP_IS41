using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Uchenik", Schema = "School")]
public class UchenikEntity
{
	// Первичный ключ
	[Key]
	public long UchenikID { get; set; }

	// Фамилия
	[Required(ErrorMessage = "Фамилия обязательна"), StringLength(100)]
	public string Familiia { get; set; }

	// Имя
	[Required(ErrorMessage = "Имя обязательно"), StringLength(100)]
	public string Imya { get; set; }

	// Отчество
	[StringLength(100)]
	public string Otchestvo { get; set; }

	// Дата рождения
	[Required(ErrorMessage = "Дата рождения обязательна")]
	public DateTime DataRozhdeniya { get; set; }

	// Адрес проживания
	[Required(ErrorMessage = "Адрес проживания обязателен"), StringLength(255)]
	public string AdresProzhivaniya { get; set; }

	// ФИО родителей
	[Required(ErrorMessage = "ФИО родителей обязательно"), StringLength(255)]
	public string FIORoditeley { get; set; }

	// Телефон родителя
	[StringLength(20)]
	public string TelefonRoditelya { get; set; }

	// Дата зачисления
	[Required(ErrorMessage = "Дата зачисления обязательна")]
	public DateTime DataZachisleniya { get; set; }

	// Класс ID
	[Required(ErrorMessage = "Класс обязателен")]
	public long KlassID { get; set; }

	/// <summary> Навигационное свойство для роли. </summary>
	[ForeignKey("KlassID")]
	public virtual KlassEntity Klass { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет ученика учебного заведения
/// </summary>
[Table("Uchenik", Schema = "School")]
public class UchenikEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long UchenikID { get; set; }

	/// <summary>
	/// Фамилия
	/// </summary>
	[Required(ErrorMessage = "Фамилия обязательна"), StringLength(100)]
	public string Familiia { get; set; }

	/// <summary>
	/// Имя
	/// </summary>
	[Required(ErrorMessage = "Имя обязательно"), StringLength(100)]
	public string Imya { get; set; }

	/// <summary>
	/// Отчество
	/// </summary>
	[StringLength(100)]
	public string Otchestvo { get; set; }

	/// <summary>
	/// Дата рождения
	/// </summary>
	[Required(ErrorMessage = "Дата рождения обязательна")]
	public DateTime DataRozhdeniya { get; set; }

	/// <summary>
	/// Адрес проживания
	/// </summary>
	[Required(ErrorMessage = "Адрес проживания обязателен"), StringLength(255)]
	public string AdresProzhivaniya { get; set; }

	/// <summary>
	/// ФИО родителей
	/// </summary>
	[Required(ErrorMessage = "ФИО родителей обязательно"), StringLength(255)]
	public string FIORoditeley { get; set; }

	/// <summary>
	/// Телефон родителя
	/// </summary>
	[StringLength(20)]
	public string TelefonRoditelya { get; set; }

	/// <summary>
	/// Дата зачисления
	/// </summary>
	[Required(ErrorMessage = "Дата зачисления обязательна")]
	public DateTime DataZachisleniya { get; set; }

	/// <summary>
	/// Класс ID
	/// </summary>
	[Required(ErrorMessage = "Класс обязателен")]
	public long KlassID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("KlassID")]
	public virtual KlassEntity Klass { get; set; }
}

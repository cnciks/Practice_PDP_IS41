using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет запись успеваемости ученика (оценки и посещаемость)
/// </summary>
[Table("ZhurnalUspevaemosti", Schema = "School")]
public class ZhurnalUspevaemostiEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long ZapisID { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на расписание занятий
	/// </summary>
	[Required(ErrorMessage = "Расписание обязательно")]
	public long RaspisanieID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("RaspisanieID")]
	public virtual RaspisanieEntity Raspisanie { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на ученика
	/// </summary>
	[Required(ErrorMessage = "Ученик обязателен")]
	public long UchenikID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	/// <summary>
	/// Дата урока
	/// </summary>
	[Required(ErrorMessage = "Дата урока обязательна")]
	public DateTime DataUroka { get; set; }

	/// <summary>
	/// Оценка (числовое значение оценки)
	/// </summary>
	public int Ocenka { get; set; }

	/// <summary>
	/// Тип оценки (контрольная работа, самостоятельная работа и т.д.)
	/// </summary>
	[Required(ErrorMessage = "Тип оценки обязателен"), StringLength(50)]
	public string TipOcenki { get; set; }

	/// <summary>
	/// Посещаемость (true - присутствовал, false - отсутствовал)
	/// </summary>
	public bool Poseschaemost { get; set; }

	/// <summary>
	/// Причина отсутствия (если отсутствует)
	/// </summary>
	[StringLength(255)]
	public string PrichinaOtssutstviya { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("ZhurnalUspevaemosti", Schema = "School")]
public class ZhurnalUspevaemostiEntity
{
	// Первичный ключ
	[Key]
	public long ZapisID { get; set; }

	// Внешний ключ, ссылка на расписание занятий
	[Required(ErrorMessage = "Расписание обязательно")]
	public long RaspisanieID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("RaspisanieID")]
	public virtual RaspisanieEntity Raspisanie { get; set; }

	// Внешний ключ, ссылка на ученика
	[Required(ErrorMessage = "Ученик обязателен")]
	public long UchenikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	// Дата урока
	[Required(ErrorMessage = "Дата урока обязательна")]
	public DateTime DataUroka { get; set; }

	// Оценка (числовое значение оценки)
	public int Ocenka { get; set; }

	// Тип оценки (контрольная работа, самостоятельная работа и т.д.)
	[Required(ErrorMessage = "Тип оценки обязателен"), StringLength(50)]
	public string TipOcenki { get; set; }

	// Посещаемость (true - присутствовал, false - отсутствовал)
	public bool Poseschaemost { get; set; }

	// Причина отсутствия (если отсутствует)
	[StringLength(255)]
	public string PrichinaOtssutstviya { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Klass", Schema = "School")]
public class KlassEntity
{
	// Первичный ключ
	[Key]
	public long KlassID { get; set; }

	// Номер класса (например, 5A, 9B)
	[Required(ErrorMessage = "Номер класса обязателен"), StringLength(10)]
	public string NomerKlassa { get; set; }

	// Учебный год
	[Required(ErrorMessage = "Учебный год обязателен")]
	public int GodObucheniya { get; set; }

	// Внешний ключ, ссылка на учителя-руководителя класса
	[Required(ErrorMessage = "Руководитель класса обязателен")]
	public long KlassRukovoditelID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("KlassRukovoditelID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }

	// Внешний ключ, ссылка на учебный план класса
	[Required(ErrorMessage = "План обязателен")]
	public long PlanID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("PlanID")]
	public virtual UchebniyPlanEntity UchebniyPlan { get; set; }
}

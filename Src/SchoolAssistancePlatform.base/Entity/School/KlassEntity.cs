using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет класс (группу учащихся) в школе
/// </summary>
[Table("Klass", Schema = "School")]
public class KlassEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long KlassID { get; set; }

	/// <summary>
	/// Номер класса (например, 5A, 9B)
	/// </summary>
	[Required(ErrorMessage = "Номер класса обязателен"), StringLength(10)]
	public string NomerKlassa { get; set; }

	/// <summary>
	/// Учебный год
	/// </summary>
	[Required(ErrorMessage = "Учебный год обязателен")]
	public int GodObucheniya { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на учителя-руководителя класса
	/// </summary>
	[Required(ErrorMessage = "Руководитель класса обязателен")]
	public long KlassRukovoditelID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("KlassRukovoditelID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на учебный план класса
	/// </summary>
	[Required(ErrorMessage = "План обязателен")]
	public long PlanID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("PlanID")]
	public virtual UchebniyPlanEntity UchebniyPlan { get; set; }
}

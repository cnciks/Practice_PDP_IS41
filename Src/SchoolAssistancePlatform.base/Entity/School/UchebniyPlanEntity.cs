using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет учебный план
/// </summary>
[Table("UchebniyPlan", Schema = "School")]
public class UchebniyPlanEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long PlanID { get; set; }

	/// <summary>
	/// Название плана
	/// </summary>
	[Required(ErrorMessage = "Название обязательно"), StringLength(255)]
	public string Nazvanie { get; set; }

	/// <summary>
	/// Год начала учебного плана
	/// </summary>
	[Required(ErrorMessage = "Год обязателен")]
	public int GodNachala { get; set; }

	/// <summary>
	/// Описание учебного плана
	/// </summary>
	[Column(TypeName = "TEXT")]
	public string Opisanie { get; set; }
}

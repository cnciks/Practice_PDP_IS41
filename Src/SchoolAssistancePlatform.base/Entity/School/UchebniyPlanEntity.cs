using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("UchebniyPlan", Schema = "School")]
public class UchebniyPlanEntity
{
	// Первичный ключ
	[Key]
	public long PlanID { get; set; }

	// Название плана
	[Required(ErrorMessage = "Название обязательно"), StringLength(255)]
	public string Nazvanie { get; set; }

	// Год начала учебного плана
	[Required(ErrorMessage = "Год обязателен")]
	public int GodNachala { get; set; }

	// Описание учебного плана
	[Column(TypeName = "TEXT")] // Для больших объемов текста
	public string Opisanie { get; set; }
}

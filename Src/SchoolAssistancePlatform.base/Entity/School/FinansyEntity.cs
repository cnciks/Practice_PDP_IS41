using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Finansy", Schema = "School")]
public class FinansyEntity
{
	// Первичный ключ
	[Key]
	public long PlatezhID { get; set; }

	// Внешний ключ, ссылка на учащийся
	[Required(ErrorMessage = "Идентификатор учащегося обязателен")]
	public long UchenikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	// Дата платежа
	[Required(ErrorMessage = "Дата платежа обязательна")]
	public DateTime DataPlatezha { get; set; }

	// Сумма платежа
	[Required(ErrorMessage = "Сумма платежа обязательна")]
	[Column(TypeName = "DECIMAL(18,2)")]
	public decimal Summa { get; set; }

	// Назначение платежа
	[Required(ErrorMessage = "Назначение платежа обязательно"), StringLength(255)]
	public string Naznachenie { get; set; }

	// Тип операции
	[Required(ErrorMessage = "Тип операции обязателен"), StringLength(50)]
	public string TipOperacii { get; set; }
}

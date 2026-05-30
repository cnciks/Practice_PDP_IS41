using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет финансовую операцию (платеж) учащегося
/// </summary>
[Table("Finansy", Schema = "School")]
public class FinansyEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long PlatezhID { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на учащегося
	/// </summary>
	[Required(ErrorMessage = "Идентификатор учащегося обязателен")]
	public long UchenikID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	/// <summary>
	/// Дата платежа
	/// </summary>
	[Required(ErrorMessage = "Дата платежа обязательна")]
	public DateTime DataPlatezha { get; set; }

	/// <summary>
	/// Сумма платежа
	/// </summary>
	[Required(ErrorMessage = "Сумма платежа обязательна")]
	[Column(TypeName = "DECIMAL(18,2)")]
	public decimal Summa { get; set; }

	/// <summary>
	/// Назначение платежа
	/// </summary>
	[Required(ErrorMessage = "Назначение платежа обязательно"), StringLength(255)]
	public string Naznachenie { get; set; }

	/// <summary>
	/// Тип операции
	/// </summary>
	[Required(ErrorMessage = "Тип операции обязателен"), StringLength(50)]
	public string TipOperacii { get; set; }
}

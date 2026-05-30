using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("DvizhenieUcheniakov", Schema = "School")]
public class DvizhenieUcheniakovEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long DvizhenieID { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на ученика
	/// </summary>
	[Required(ErrorMessage = "Идентификатор ученика обязателен")]
	public long UchenikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	/// <summary>
	/// Дата изменения
	/// </summary>
	[Required(ErrorMessage = "Дата изменения обязательна")]
	public DateTime DataIzmeneniya { get; set; }

	/// <summary>
	/// Тип движения (например, перевод, отчисление, восстановление)
	/// </summary>
	[Required(ErrorMessage = "Тип движения обязателен"), StringLength(50)]
	public string TipDvizheniya { get; set; }

	/// <summary>
	/// Основание (причина) движения
	/// </summary>
	[Required(ErrorMessage = "Основание обязательно"), StringLength(255)]
	public string Osnovanie { get; set; }

	/// <summary>
	/// Дополнительные комментарии
	/// </summary>
	[StringLength(500)]
	public string Kommentariy { get; set; }
}

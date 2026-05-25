using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("DvizhenieUcheniakov", Schema = "School")]
public class DvizhenieUcheniakovEntity
{
	// Первичный ключ
	[Key]
	public long DvizhenieID { get; set; }

	// Внешний ключ, ссылка на ученика
	[Required(ErrorMessage = "Идентификатор ученика обязателен")]
	public long UchenikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("UchenikID")]
	public virtual UchenikEntity Uchenik { get; set; }

	// Дата изменения
	[Required(ErrorMessage = "Дата изменения обязательна")]
	public DateTime DataIzmeneniya { get; set; }

	// Тип движения (например, перевод, отчисление, восстановление)
	[Required(ErrorMessage = "Тип движения обязателен"), StringLength(50)]
	public string TipDvizheniya { get; set; }

	// Основание (причина) движения
	[Required(ErrorMessage = "Основание обязательно"), StringLength(255)]
	public string Osnovanie { get; set; }

	// Дополнительные комментарии
	[StringLength(500)] // Допустимо больше текста
	public string Kommentariy { get; set; }
}

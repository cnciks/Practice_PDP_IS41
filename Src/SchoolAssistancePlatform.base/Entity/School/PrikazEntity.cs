using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Prikaz", Schema = "School")]
public class PrikazEntity
{
	// Первичный ключ
	[Key]
	public int PrikazID { get; set; }

	// Номер приказа
	[Required(ErrorMessage = "Номер приказа обязателен"), StringLength(50)]
	public string NomerPrikaza { get; set; }

	// Дата приказа
	[Required(ErrorMessage = "Дата приказа обязательна")]
	public DateTime DataPrikaza { get; set; }

	// Тип приказа (например, приказ о зачислении, переводе и т.д.)
	[Required(ErrorMessage = "Тип приказа обязателен"), StringLength(100)]
	public string TipPrikaza { get; set; }

	// Содержание приказа
	[Required(ErrorMessage = "Содержание обязательно"), Column(TypeName = "TEXT")]
	public string Soderzhanie { get; set; }

	// Ссылка на файл документа
	[StringLength(255)]
	public string FileLink { get; set; }

	// Внешний ключ, ссылка на сотрудника, издавшего приказ
	[Required(ErrorMessage = "Ссылка на сотрудника обязательна")]
	public long SotrudnikID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("SotrudnikID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }
}

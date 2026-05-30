using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет приказ (распорядительный документ)
/// </summary>
[Table("Prikaz", Schema = "School")]
public class PrikazEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public int PrikazID { get; set; }

	/// <summary>
	/// Номер приказа
	/// </summary>
	[Required(ErrorMessage = "Номер приказа обязателен"), StringLength(50)]
	public string NomerPrikaza { get; set; }

	/// <summary>
	/// Дата приказа
	/// </summary>
	[Required(ErrorMessage = "Дата приказа обязательна")]
	public DateTime DataPrikaza { get; set; }

	/// <summary>
	/// Тип приказа (например, приказ о зачислении, переводе и т.д.)
	/// </summary>
	[Required(ErrorMessage = "Тип приказа обязателен"), StringLength(100)]
	public string TipPrikaza { get; set; }

	/// <summary>
	/// Содержание приказа
	/// </summary>
	[Required(ErrorMessage = "Содержание обязательно"), Column(TypeName = "TEXT")]
	public string Soderzhanie { get; set; }

	/// <summary>
	/// Ссылка на файл документа
	/// </summary>
	[StringLength(255)]
	public string FileLink { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на сотрудника, издавшего приказ
	/// </summary>
	[Required(ErrorMessage = "Ссылка на сотрудника обязательна")]
	public long SotrudnikID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("SotrudnikID")]
	public virtual SotrudnikEntity Sotrudnik { get; set; }
}

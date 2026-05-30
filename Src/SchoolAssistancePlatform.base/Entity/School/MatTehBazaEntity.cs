using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет материально-техническую базу (имущество, оборудование, мебель)
/// </summary>
[Table("MatTehBaza", Schema = "School")]
public class MatTehBazaEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public int InventarID { get; set; }

	/// <summary>
	/// Наименование имущества
	/// </summary>
	[Required(ErrorMessage = "Наименование обязательно"), StringLength(255)]
	public string Naimenovanie { get; set; }

	/// <summary>
	/// Тип имущества (оборудование, мебель и т.д.)
	/// </summary>
	[Required(ErrorMessage = "Тип обязателен"), StringLength(50)]
	public string Tip { get; set; }

	/// <summary>
	/// Кабинет размещения
	/// </summary>
	[Required(ErrorMessage = "Кабинет обязателен"), StringLength(10)]
	public string Kabinet { get; set; }

	/// <summary>
	/// Инвентарный номер
	/// </summary>
	[Required(ErrorMessage = "Инвентарный номер обязателен"), StringLength(20)]
	public string InvNomer { get; set; }

	/// <summary>
	/// Статус имущества (в наличии, списано и т.д.)
	/// </summary>
	[Required(ErrorMessage = "Статус обязателен"), StringLength(50)]
	public string Status { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на приказ о постановке на учет
	/// </summary>
	[Required(ErrorMessage = "Приказ постановки обязателен")]
	public int PrikazPostupleniyaID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("PrikazPostupleniyaID")]
	public virtual PrikazEntity PrikazPostupleniya { get; set; }

	/// <summary>
	/// Внешний ключ, ссылка на приказ о списании
	/// </summary>
	public int? PrikazSpisaniyaID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("PrikazSpisaniyaID")]
	public virtual PrikazEntity PrikazSpisaniya { get; set; }
}

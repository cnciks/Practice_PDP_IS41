using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("MatTehBaza", Schema = "School")]
public class MatTehBazaEntity
{
	// Первичный ключ
	[Key]
	public int InventarID { get; set; }

	// Наименование имущества
	[Required(ErrorMessage = "Наименование обязательно"), StringLength(255)]
	public string Naimenovanie { get; set; }

	// Тип имущества (оборудование, мебель и т.д.)
	[Required(ErrorMessage = "Тип обязателен"), StringLength(50)]
	public string Tip { get; set; }

	// Кабинет размещения
	[Required(ErrorMessage = "Кабинет обязателен"), StringLength(10)]
	public string Kabinet { get; set; }

	// Инвентарный номер
	[Required(ErrorMessage = "Инвентарный номер обязателен"), StringLength(20)]
	public string InvNomer { get; set; }

	// Статус имущества (в наличии, списано и т.д.)
	[Required(ErrorMessage = "Статус обязателен"), StringLength(50)]
	public string Status { get; set; }

	// Внешний ключ, ссылка на приказ о постановке на учет
	[Required(ErrorMessage = "Приказ постановки обязателен")]
	public int PrikazPostupleniyaID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("PrikazPostupleniyaID")]
	public virtual PrikazEntity PrikazPostupleniya { get; set; }

	// Внешний ключ, ссылка на приказ о списании
	public int? PrikazSpisaniyaID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("PrikazSpisaniyaID")]
	public virtual PrikazEntity PrikazSpisaniya { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Soobschenie", Schema = "School")]
public class SoobschenieEntity
{
	// Первичный ключ
	[Key]
	public int SoobschenieID { get; set; }

	// Внешний ключ, отправитель сообщения
	[Required(ErrorMessage = "Отправитель обязателен")]
	public long OtpravitelID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("OtpravitelID")]
	public virtual SotrudnikEntity Otpravitel { get; set; }

	// Внешний ключ, получатель сообщения
	[Required(ErrorMessage = "Получатель обязателен")]
	public long PoluchatelID { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("PoluchatelID")]
	public virtual SotrudnikEntity Poluchatel { get; set; }

	// Тип получателя (ученик, учитель, родитель и т.д.)
	[Required(ErrorMessage = "Тип получателя обязателен"), StringLength(50)]
	public string TipPoluchatelya { get; set; }

	// Тема сообщения
	[Required(ErrorMessage = "Тема обязательна"), StringLength(255)]
	public string Tema { get; set; }

	// Текст сообщения
	[Required(ErrorMessage = "Текст обязателен"), Column(TypeName = "TEXT")]
	public string Text { get; set; }

	// Дата отправки
	[Required(ErrorMessage = "Дата отправки обязательна")]
	public DateTime DataOtpravki { get; set; }

	// Прочитано (булево поле, true - прочитано, false - непрочитано)
	public bool Prochitano { get; set; }
}

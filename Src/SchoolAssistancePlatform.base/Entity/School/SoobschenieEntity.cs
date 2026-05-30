using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет сообщение (внутреннюю переписку между сотрудниками системы)
/// </summary>
[Table("Soobschenie", Schema = "School")]
public class SoobschenieEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public int SoobschenieID { get; set; }

	/// <summary>
	/// Внешний ключ, отправитель сообщения
	/// </summary>
	[Required(ErrorMessage = "Отправитель обязателен")]
	public long OtpravitelID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("OtpravitelID")]
	public virtual SotrudnikEntity Otpravitel { get; set; }

	/// <summary>
	/// Внешний ключ, получатель сообщения
	/// </summary>
	[Required(ErrorMessage = "Получатель обязателен")]
	public long PoluchatelID { get; set; }

	/// <summary>
	/// Навигационное свойство
	/// </summary>
	[ForeignKey("PoluchatelID")]
	public virtual SotrudnikEntity Poluchatel { get; set; }

	/// <summary>
	/// Тип получателя (ученик, учитель, родитель и т.д.)
	/// </summary>
	[Required(ErrorMessage = "Тип получателя обязателен"), StringLength(50)]
	public string TipPoluchatelya { get; set; }

	/// <summary>
	/// Тема сообщения
	/// </summary>
	[Required(ErrorMessage = "Тема обязательна"), StringLength(255)]
	public string Tema { get; set; }

	/// <summary>
	/// Текст сообщения
	/// </summary>
	[Required(ErrorMessage = "Текст обязателен"), Column(TypeName = "TEXT")]
	public string Text { get; set; }

	/// <summary>
	/// Дата отправки
	/// </summary>
	[Required(ErrorMessage = "Дата отправки обязательна")]
	public DateTime DataOtpravki { get; set; }

	/// <summary>
	/// Прочитано (булево поле, true - прочитано, false - непрочитано)
	/// </summary>
	public bool Prochitano { get; set; }
}

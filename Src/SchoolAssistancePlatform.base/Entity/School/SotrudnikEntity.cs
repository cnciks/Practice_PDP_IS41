using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

/// <summary>
/// Представляет сотрудника учебного заведения
/// </summary>
[Table("Sotrudnik", Schema = "School")]
public class SotrudnikEntity
{
	/// <summary>
	/// Первичный ключ
	/// </summary>
	[Key]
	public long SotrudnikID { get; set; }

	/// <summary>
	/// Фамилия
	/// </summary>
	[Required(ErrorMessage = "Фамилия обязательна"), StringLength(100)]
	public string Familia { get; set; }

	/// <summary>
	/// Имя
	/// </summary>
	[Required(ErrorMessage = "Имя обязательно"), StringLength(100)]
	public string Imya { get; set; }

	/// <summary>
	/// Отчество
	/// </summary>
	[StringLength(100)]
	public string Otchestvo { get; set; }

	/// <summary>
	/// Дата рождения
	/// </summary>
	[Required(ErrorMessage = "Дата рождения обязательна")]
	public DateTime DataRozhdeniya { get; set; }

	/// <summary>
	/// Должность
	/// </summary>
	[Required(ErrorMessage = "Должность обязательна"), StringLength(100)]
	public string Dolzhnost { get; set; }

	/// <summary>
	/// Email
	/// </summary>
	[StringLength(255)]
	public string Email { get; set; }

	/// <summary>
	/// Телефон
	/// </summary>
	[StringLength(20)]
	public string Telefon { get; set; }

	/// <summary>
	/// Дата приема
	/// </summary>
	[Required(ErrorMessage = "Дата приема обязательна")]
	public DateTime DataPriema { get; set; }

	/// <summary>
	/// Статус
	/// </summary>
	[StringLength(50)]
	public string Status { get; set; }
}

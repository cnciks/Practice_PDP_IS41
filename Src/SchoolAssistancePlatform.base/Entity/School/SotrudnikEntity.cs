using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.School;

[Table("Sotrudnik", Schema = "School")]
public class SotrudnikEntity
{
	// Первичный ключ
	[Key]
	public long SotrudnikID { get; set; }

	// Фамилия
	[Required(ErrorMessage = "Фамилия обязательна"), StringLength(100)]
	public string Familia { get; set; }

	// Имя
	[Required(ErrorMessage = "Имя обязательно"), StringLength(100)]
	public string Imya { get; set; }

	// Отчество
	[StringLength(100)]
	public string Otchestvo { get; set; }

	// Дата рождения
	[Required(ErrorMessage = "Дата рождения обязательна")]
	public DateTime DataRozhdeniya { get; set; }

	// Должность
	[Required(ErrorMessage = "Должность обязательна"), StringLength(100)]
	public string Dolzhnost { get; set; }

	// Email
	[StringLength(255)]
	public string Email { get; set; }

	// Телефон
	[StringLength(20)]
	public string Telefon { get; set; }

	// Дата приема
	[Required(ErrorMessage = "Дата приема обязательна")]
	public DateTime DataPriema { get; set; }

	// Статус
	[StringLength(50)]
	public string Status { get; set; }
}

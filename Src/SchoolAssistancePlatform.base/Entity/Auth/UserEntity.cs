using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SchoolAssistancePlatform.Base.Entity.School;

namespace SchoolAssistancePlatform.Base.Entity.Auth;

// Класс пользователя
[Table("Users", Schema = "Auth")]
public class UserEntity
{
	/// <summary> Идентификатор пользователя. </summary>
	[Key]
	public long Id { get; set; }

	/// <summary> Логин пользователя. </summary>
	[Required]
	[MaxLength(50)]
	public string Login { get; set; }

	/// <summary> Хеш пароля. </summary>
	[Required]
	[MaxLength(255)]
	public string PasswordHash { get; set; }

	/// <summary> Электронная почта. </summary>
	[MaxLength(100)]
	public string Email { get; set; }

	/// <summary> Внешний ключ к сотрудника. </summary>
	public long? SotrudnikId { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("SotrudnikId")]
	public virtual SotrudnikEntity? Sotrudnik { get; set; }

	/// <summary> Внешний ключ к роли. </summary>
	public long RoleId { get; set; }

	/// <summary> Навигационное свойство. </summary>
	[ForeignKey("RoleId")]
	public virtual RoleEntity Role { get; set; }
}

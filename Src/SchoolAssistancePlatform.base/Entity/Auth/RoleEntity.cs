using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.Auth;

// Класс роли
[Table("Roles", Schema = "Auth")]
public class RoleEntity
{
	/// <summary> Идентификатор роли. </summary>
	[Key]
	public long Id { get; set; }

	/// <summary> Название роли. </summary>
	[Required]
	[MaxLength(50)]
	public string Name { get; set; }

	/// <summary> Описание роли. </summary>s
	[MaxLength(255)]
	public string Description { get; set; }

	public virtual ICollection<RolePermissionEntity> Permissions { get; set; } = [];
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAssistancePlatform.Base.Entity.Auth;

[Table("RolePermissions", Schema = "Auth")]
public class RolePermissionEntity
{
	[Key]
	public long Id { get; set; }

	public long RoleId { get; set; }

	public long PermissionValue { get; set; }

	[ForeignKey("RoleId")]
	public virtual RoleEntity Role { get; set; }
}

using Mapster;

namespace SchoolAssistancePlatform.framework.Data;

[AdaptTo("[name]Entity")]
public class UserDto
{
	public long Id { get; set; }

	public string Login { get; set; }

	public string Email { get; set; }

	[AdaptMember("Role")]
	public RoleDto Role { get; set; }

	public long?  SotrudnikId  { get; set; }
	public string? SotrudnikFIO { get; set; }
}

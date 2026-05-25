namespace SchoolAssistancePlatform.framework.Data;

public class RoleDto
{
	public long Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public List<Permissions> Permissions { get; set; } = [];
}

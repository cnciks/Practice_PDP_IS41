using System.Linq;

using Mapster;

using SchoolAssistancePlatform.Base.Entity.Auth;
using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI;

public static class MapsterConfig
{
	public static void Configure()
	{
		TypeAdapterConfig<UserEntity, UserDto>
			.NewConfig()
			.TwoWays()
			.PreserveReference(true);

		TypeAdapterConfig<RoleEntity, RoleDto>.NewConfig()
			.Map(dest => dest.Permissions, src =>
				src.Permissions.Select(p => (Permissions)p.PermissionValue).ToList());

		TypeAdapterConfig<RoleDto, RoleEntity>.NewConfig()
			.Map(dest => dest.Permissions, src =>
				src.Permissions.Select(perm => new RolePermissionEntity
				{
					PermissionValue = (long)perm
				}).ToList());
	}
}

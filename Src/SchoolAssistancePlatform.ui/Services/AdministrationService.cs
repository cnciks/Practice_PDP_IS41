using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Entity.Auth;
using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.UI.Services;

public sealed class AdministrationService(IComponentContext componentContext)
{
	private readonly IComponentContext _ctx = componentContext;

	private SAPDbContext Db => _ctx.Resolve<SAPDbContext>();

	public async Task<List<UserDto>> GetAllUsersAsync()
	{
		var db = Db;

		var users = await db.Users
			.AsNoTracking()
			.Include(u => u.Role)
				.ThenInclude(r => r.Permissions)
			.Include(u => u.Sotrudnik)
			.ToListAsync();

		return users.Select(u => new UserDto
		{
			Id          = u.Id,
			Login       = u.Login,
			Email       = u.Email ?? string.Empty,
			SotrudnikId = u.SotrudnikId,
			SotrudnikFIO = u.Sotrudnik is null ? null
				: $"{u.Sotrudnik.Familia} {u.Sotrudnik.Imya} {u.Sotrudnik.Otchestvo}".Trim(),
			Role  = new RoleDto
			{
				Id          = u.Role.Id,
				Name        = u.Role.Name,
				Description = u.Role.Description ?? string.Empty,
				Permissions = u.Role.Permissions
					.Select(p => (Permissions)p.PermissionValue)
					.ToList(),
			},
		}).ToList();
	}

	public async Task<List<SotrudnikDto>> GetAllEmployeesAsync()
	{
		var db = Db;
		return await db.Sotrudniki
			.AsNoTracking()
			.OrderBy(s => s.Familia)
			.Select(s => new SotrudnikDto
			{
				SotrudnikID  = s.SotrudnikID,
				Familia      = s.Familia,
				Imya         = s.Imya,
				Otchestvo    = s.Otchestvo ?? string.Empty,
				Dolzhnost    = s.Dolzhnost ?? string.Empty,
				Email        = s.Email ?? string.Empty,
				Telefon      = s.Telefon ?? string.Empty,
				Status       = s.Status ?? string.Empty,
			})
			.ToListAsync();
	}

	public async Task<UserDto> CreateUserAsync(string login, string password, string email, long roleId, long? sotrudnikId)
	{
		var db = Db;

		var entity = new UserEntity
		{
			Login        = login,
			PasswordHash = password,
			Email        = email,
			RoleId       = roleId,
			SotrudnikId  = sotrudnikId == 0 ? null : sotrudnikId,
		};

		db.Users.Add(entity);
		await db.SaveChangesAsync();

		return (await GetAllUsersAsync()).First(u => u.Id == entity.Id);
	}

	public async Task<bool> DeleteUserAsync(long id)
	{
		var db   = Db;
		var user = await db.Users.FindAsync(id);
		if(user is null) return false;

		db.Users.Remove(user);
		await db.SaveChangesAsync();
		return true;
	}

	public async Task UpdateUserAsync(long id, string email, long roleId)
	{
		var db   = Db;
		var user = await db.Users.FindAsync(id);
		if(user is null) return;

		user.Email  = email;
		user.RoleId = roleId;
		await db.SaveChangesAsync();
	}

	public async Task<List<RoleDto>> GetAllRolesAsync()
	{
		var db = Db;
		return await db.Roles
			.AsNoTracking()
			.Include(r => r.Permissions)
			.Select(r => new RoleDto
			{
				Id          = r.Id,
				Name        = r.Name,
				Description = r.Description ?? string.Empty,
				Permissions = r.Permissions
					.Select(p => (Permissions)p.PermissionValue)
					.ToList(),
			})
			.ToListAsync();
	}

	public async Task<RoleDto> CreateRoleAsync(string name, string description, IEnumerable<Permissions> permissions)
	{
		var db = Db;

		var entity = new RoleEntity
		{
			Name        = name,
			Description = description,
		};

		foreach(var perm in permissions)
			entity.Permissions.Add(new RolePermissionEntity { PermissionValue = (long)perm });

		db.Roles.Add(entity);
		await db.SaveChangesAsync();

		return (await GetAllRolesAsync()).First(r => r.Id == entity.Id);
	}

	public async Task<bool> DeleteRoleAsync(long id)
	{
		var db   = Db;
		var role = await db.Roles
			.Include(r => r.Permissions)
			.FirstOrDefaultAsync(r => r.Id == id);

		if(role is null) return false;

		var hasUsers = await db.Users.AnyAsync(u => u.RoleId == id);
		if(hasUsers) return false;

		db.Roles.Remove(role);
		await db.SaveChangesAsync();
		return true;
	}

	public async Task UpdateRolePermissionsAsync(long roleId, IEnumerable<Permissions> permissions)
	{
		var db   = Db;
		var role = await db.Roles
			.Include(r => r.Permissions)
			.FirstOrDefaultAsync(r => r.Id == roleId);

		if(role is null) return;

		db.RolePermissions.RemoveRange(role.Permissions);

		foreach(var perm in permissions)
			role.Permissions.Add(new RolePermissionEntity { RoleId = roleId, PermissionValue = (long)perm });

		await db.SaveChangesAsync();
	}
}

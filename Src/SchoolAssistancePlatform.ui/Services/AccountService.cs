using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using Mapster;

using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Entity.Auth;
using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;
using SchoolAssistancePlatform.framework.Interfaces;

namespace SchoolAssistancePlatform.UI.Services;

internal class AccountService(IComponentContext componentContext) : IInitializer
{
	private static readonly Permissions[] allPermissionsArray =
	[
		Permissions.MainPage,
		Permissions.AdministrationPage,
		Permissions.FinancePage,
		Permissions.CurriculumPage,
		Permissions.SchedulePage,
		Permissions.JournalPage,
		Permissions.EmployeesPage,
		Permissions.StudentsPage,
		Permissions.MessagesPage,
		Permissions.CommandsPage,
		Permissions.ReportsPage,
		Permissions.ClassesPage
	];

	#region Data

	readonly IComponentContext _componentContext = componentContext;

	readonly string _adminLogin = "admin";

	readonly string _adminPassword = "admin";

	#endregion

	public event EventHandler? AccountChange;

	public UserDto? AccountUser { get; private set; }

	public bool IsLogIn => AccountUser != null;

	public static Permissions[] AllPermissionsArray => allPermissionsArray;

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		var context = _componentContext.Resolve<SAPDbContext>();

		var adminRole = await context.Roles
			.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);

		if(adminRole == null)
		{
			adminRole = new RoleEntity { Name = "Admin", Description = "Системный администратор" };

			foreach(var perm in AllPermissionsArray)
			{
				adminRole.Permissions.Add(new RolePermissionEntity
				{
					PermissionValue = (long)perm
				});
			}

			context.Roles.Add(adminRole);

			await context.SaveChangesAsync(cancellationToken);
		}

		var adminUser = await context.Users
			.FirstOrDefaultAsync(u => u.Login == _adminLogin, cancellationToken);

		if(adminUser == null)
		{
			adminUser = new UserEntity
			{
				Login        = _adminLogin,
				Email        = "admin@admin.ru",
				RoleId       = adminRole.Id,
				PasswordHash = _adminPassword
			};

			context.Users.Add(adminUser);

			await context.SaveChangesAsync(cancellationToken);
		}
	}

	internal Task<bool> CheckUserAsync(string login, string password)
	{
		var context = _componentContext.Resolve<SAPDbContext>();

		var user = context.Users
			.AsNoTracking()
			.Where(u => u.Login == login && u.PasswordHash == password);

		return user.AnyAsync();
	}

	internal async Task ChangeAccountAsync(string login, string password)
	{
		var context = _componentContext.Resolve<SAPDbContext>();

		var userEntity = await context.Users
			.AsNoTracking()
			.Include(u => u.Role)
				.ThenInclude(r => r.Permissions)
			.FirstAsync(u => u.Login == login && u.PasswordHash == password);

		AccountUser = userEntity
			.Adapt<UserDto>();

		AccountChange?.Invoke(this, EventArgs.Empty);
	}
}

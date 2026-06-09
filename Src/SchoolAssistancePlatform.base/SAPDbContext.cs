using Microsoft.EntityFrameworkCore;

using SchoolAssistancePlatform.Base.Configurations;
using SchoolAssistancePlatform.Base.Entity.Auth;
using SchoolAssistancePlatform.Base.Entity.School;

namespace SchoolAssistancePlatform.Base;

public sealed class SAPDbContext(DatabaseSettings databaseSettings) : DbContext
{
	private readonly string _connectionString = databaseSettings.ConnectionString;

	public DbSet<UserEntity> Users { get; set; }

	public DbSet<RoleEntity> Roles { get; set; }

	public DbSet<RolePermissionEntity> RolePermissions { get; set; }

	public DbSet<SotrudnikEntity> Sotrudniki { get; set; }

	public DbSet<UchenikEntity> Uchenik { get; set; }

	public DbSet<FinansyEntity> Finansy { get; set; }

	public DbSet<DvizhenieUcheniakovEntity> DvizhenieUcheniakov { get; set; }

	public DbSet<UchebniyPredmetEntity> UchebniyPredmet { get; set; }

	public DbSet<UchebniyPlanEntity> UchebniyPlan { get; set; }

	public DbSet<KlassEntity> Klassy { get; set; }

	public DbSet<RaspisanieEntity> Raspisanie { get; set; }

	public DbSet<PrikazEntity> Prikazy { get; set; }

	public DbSet<MatTehBazaEntity> MatTehBaza { get; set; }

	public DbSet<SoobschenieEntity> Soobschenie { get; set; }

	public DbSet<ZhurnalUspevaemostiEntity> ZhurnalUspevaemosti { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(_connectionString);
	}
}

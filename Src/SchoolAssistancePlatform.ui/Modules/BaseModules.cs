using System;

using Autofac;

using Mapster;

using MapsterMapper;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Interfaces;
using SchoolAssistancePlatform.Base.Repositories;

namespace SchoolAssistancePlatform.UI.Modules;

internal class BaseModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<SAPDbContext>()
			.InstancePerLifetimeScope();

		builder.RegisterInstance(new Mapper(TypeAdapterConfig.GlobalSettings))
			.As<IMapper>()
			.SingleInstance();

		builder.RegisterType<SotrudnikRepository>()
			.As<ISotrudnikRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UchebniyPlanRepository>()
			.As<IUchebniyPlanRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<KlassRepository>()
			.As<IKlassRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UchenikRepository>()
			.As<IUchenikRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UchebniyPredmetRepository>()
			.As<IUchebniyPredmetRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<RaspisaniyaRepository>()
			.As<IRaspisanieRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ZhurnalUspevaemostiRepository>()
			.As<IZhurnalUspevaemostiRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<SoobschenieRepository>()
			.As<ISoobschenieRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<PrikazRepository>()
			.As<IPrikazRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<MatTehBazaRepository>()
			.As<IMatTehBazaRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<FinansyRepository>()
			.As<IFinansyRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<DvizhenieUcheniakovRepository>()
			.As<IDvizhenieUcheniakovRepository>()
			.InstancePerLifetimeScope();

		MapsterConfig.Configure();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
	}
}

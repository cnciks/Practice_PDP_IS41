using Autofac;

using SchoolAssistancePlatform.Base;

namespace SchoolAssistancePlatform.UI.Modules;

internal class BaseModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<SAPDbContext>()
			.InstancePerLifetimeScope();
	}
}

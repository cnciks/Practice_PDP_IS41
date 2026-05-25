using Autofac;

using SchoolAssistancePlatform.framework.Interfaces;
using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Modules;

internal class ServiceModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<AccountService>()
			.AsSelf()
			.As<IInitializer>()
			.SingleInstance();

		builder
			.RegisterType<MenuActions>()
			.AsSelf()
			.SingleInstance();
	}
}

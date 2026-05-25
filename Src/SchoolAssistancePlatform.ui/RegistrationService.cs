using Autofac;

using SchoolAssistancePlatform.Base.Configurations;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI;

internal static class RegistrationService
{
	public static IContainer CreateContainer(DatabaseSettings databaseSettings)
	{
		var builder = new ContainerBuilder();

		builder.RegisterInstance(databaseSettings);

		builder
			.RegisterType<AppBootstrapper>()
			.AsSelf()
			.As<IAppBootstrapper>()
			.SingleInstance()
			.ExternallyOwned();

		builder.RegisterAssemblyModules(typeof(RegistrationService).Assembly);

		return builder.Build();
	}
}

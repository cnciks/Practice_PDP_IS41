using Autofac;

using Avalonia;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.Configuration;

using SchoolAssistancePlatform.Base;
using SchoolAssistancePlatform.Base.Configurations;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI;

public partial class App : Application
{
	public IContainer Container { get; private set; }

	internal IAppBootstrapper AppBootstrapper { get; private set; }

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void RegisterServices()
	{
		base.RegisterServices();

		var builder = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

		IConfigurationRoot configuration = builder.Build();

		var databaseSettings = new DatabaseSettings();
		configuration.GetSection("DatabaseSettings").Bind(databaseSettings);

		Container = RegistrationService.CreateContainer(databaseSettings);

		AppBootstrapper = Container.Resolve<IAppBootstrapper>();

		var sapDbContext = Container.Resolve<SAPDbContext>();

		sapDbContext.Database.EnsureCreated();
	}

	public override async void OnFrameworkInitializationCompleted()
	{
		await AppBootstrapper.RunAsync();
	}
}

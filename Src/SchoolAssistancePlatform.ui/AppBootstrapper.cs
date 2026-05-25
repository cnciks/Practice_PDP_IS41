using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

using SchoolAssistancePlatform.framework.Interfaces;
using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI;

internal class AppBootstrapper(IComponentContext componentContext) : IAppBootstrapper
{
	public async Task RunAsync(CancellationToken cancellation = default)
	{
		if(Application.Current is Application current)
		{
			var mainWindow   = componentContext.Resolve<MainWindow>();
			var initializers = componentContext.Resolve<IReadOnlyList<IInitializer>>();

			foreach(var initializer in initializers)
			{
				await initializer.InitializeAsync(cancellation);
			}

			if(current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = mainWindow;
			}

			mainWindow.Show();
		}
	}
}

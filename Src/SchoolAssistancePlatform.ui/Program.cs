using System;

using Avalonia;

using ReactiveUI.Avalonia;

namespace SchoolAssistancePlatform.UI;

internal class Program
{
	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI(rxui =>
			{
				// Optional: add custom registration here via rxui.WithRegistration(...)
			})
			// Windows
			.With(new Win32PlatformOptions { OverlayPopups = true })
			// Unix/Linux
			.With(new X11PlatformOptions { OverlayPopups = true })
			// Mac?
			.With(new AvaloniaNativePlatformOptions { OverlayPopups = true });

	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		var builder = BuildAvaloniaApp();

		builder.StartWithClassicDesktopLifetime(args);
	}
}

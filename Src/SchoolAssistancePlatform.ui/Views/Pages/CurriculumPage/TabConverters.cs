using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;

internal sealed class TabColorConverter : IValueConverter
{
	public static readonly TabColorConverter Instance = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value is true ? new SolidColorBrush(Color.Parse("#3498DB")) : new SolidColorBrush(Color.Parse("#FFFFFF"));

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}

internal sealed class TabFgConverter : IValueConverter
{
	public static readonly TabFgConverter Instance = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value is true ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Color.Parse("#555555"));

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}

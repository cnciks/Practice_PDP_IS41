using Avalonia.Controls;

using SchoolAssistancePlatform.UI.Interfaces;

namespace SchoolAssistancePlatform.UI.Views.WorkSpace;

internal class MenuItem
{
	public IWorkSpacePage Page { get; init; }

	public Control Control { get; init; }
}

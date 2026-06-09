using System.Threading.Tasks;

using Avalonia.Media.Imaging;

using SchoolAssistancePlatform.framework;

namespace SchoolAssistancePlatform.UI.Interfaces;

internal interface IWorkSpacePage
{
	string Title { get; }

	Bitmap? Icon { get; }

	Permissions Permission { get; }

	Task LoadPageAsync();
}

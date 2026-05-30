using System.Threading.Tasks;

using SchoolAssistancePlatform.framework;

namespace SchoolAssistancePlatform.UI.Interfaces;

internal interface IWorkSpacePage
{
	string Title { get; }

	Permissions Permission { get; }

	Task LoadPageAsync();
}

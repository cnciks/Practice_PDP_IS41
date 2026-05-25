using System.Threading;
using System.Threading.Tasks;

namespace SchoolAssistancePlatform.UI.Interfaces;

internal interface IAppBootstrapper
{
	Task RunAsync(CancellationToken cancellation = default);
}

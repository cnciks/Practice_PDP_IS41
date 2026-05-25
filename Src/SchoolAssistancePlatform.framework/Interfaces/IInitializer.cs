namespace SchoolAssistancePlatform.framework.Interfaces;

public interface IInitializer
{
	Task InitializeAsync(CancellationToken cancellationToken = default);
}

namespace Runner.Configuration.Core;

public interface IAsyncInitializable
{
    public Task InitializeAsync();
}
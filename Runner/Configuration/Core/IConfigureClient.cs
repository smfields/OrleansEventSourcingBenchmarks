namespace Runner.Configuration.Core;

public interface IConfigureClient
{
    public void ConfigureClient(IClientBuilder clientBuilder);
}
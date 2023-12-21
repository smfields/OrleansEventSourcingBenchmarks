using Orleans.Concurrency;
using Orleans.EventSourcing;

namespace Runner.Grains;

public record ValueUpdatedEvent(string NewValue);

[Serializable]
[GenerateSerializer]
public class DataGrainState
{
    public string Value { get; private set; } = "";

    public void Apply(ValueUpdatedEvent valueUpdatedEvent)
    {
        Value = valueUpdatedEvent.NewValue;
    }
}

public interface IDataGrain : IGrainWithGuidKey
{
    public ValueTask Update(string value, bool confirmEvents);
    public ValueTask<string> GetCurrentValue();
    public ValueTask Deactivate();
    public Task ConfirmEvents();
}

public class DataGrain : JournaledGrain<DataGrainState>, IDataGrain
{
    public async ValueTask Update(string value, bool confirmEvents)
    {
        RaiseEvent(new ValueUpdatedEvent(value));

        if (confirmEvents)
        {
            await ConfirmEvents();
        }
    }

    public ValueTask<string> GetCurrentValue()
    {
        return ValueTask.FromResult(State.Value);
    }

    public ValueTask Deactivate()
    {
        DeactivateOnIdle();
        return ValueTask.CompletedTask;
    }

    Task IDataGrain.ConfirmEvents() => ConfirmEvents();
}

[Reentrant]
public class DataGrainReentrant : DataGrain;

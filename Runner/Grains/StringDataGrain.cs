using Orleans.EventSourcing;

namespace Runner.Grains;

public record ValueUpdatedEvent(string NewValue);

public class StringDataGrainState
{
    public string Value { get; private set; } = "";

    public void Apply(ValueUpdatedEvent valueUpdatedEvent)
    {
        Value = valueUpdatedEvent.NewValue;
    }
}

public interface IStringDataGrain : IGrainWithGuidKey
{
    public ValueTask Update(string value, bool confirmEvents);
    public ValueTask<string> GetCurrentValue();
    public ValueTask Deactivate();
}

public class StringDataGrain : JournaledGrain<StringDataGrainState>, IStringDataGrain
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
}

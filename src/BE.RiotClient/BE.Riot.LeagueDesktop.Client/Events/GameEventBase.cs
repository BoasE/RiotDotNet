namespace BE.Riot.Console.Events;

public abstract class GameEventBase : IInGameEvent
{
    protected GameEventBase(float eventTime)
    {
        GameTime = eventTime;
        Timestamp = DateTimeOffset.UtcNow;
    }

    public abstract string Name { get; }
    public DateTimeOffset Timestamp { get; }
    public float GameTime { get; }
}
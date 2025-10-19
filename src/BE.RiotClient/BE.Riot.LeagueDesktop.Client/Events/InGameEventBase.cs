namespace BE.Riot.Console.Events;

public abstract class InGameEventBase : IInGameEvent
{
    public string Name { get; protected set; } = "";
    public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.Now;
    public float GameTime { get; protected set; }

    protected InGameEventBase(string name, float gameTime = 0f)
    {
        Name = name;
        GameTime = gameTime;
        Timestamp = DateTimeOffset.Now;
    }

    protected string Stamp() => Timestamp.ToLocalTime().ToString("HH:mm:ss");
    protected string GT() => GameTime > 0 ? $" (t={GameTime:0.0}s)" : "";
    public override string ToString() => $"[{Stamp()}] {Name}{GT()}";
}
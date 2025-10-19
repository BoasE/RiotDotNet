namespace BE.Riot.Console.Events;

public interface IInGameEvent
{
    string Name { get; }
    DateTimeOffset Timestamp { get; }
    float GameTime { get; }
}
namespace BE.Riot.MatchHistories;

public interface ICompletedGameDetailCache
{
    Task<string?> GetCompletedGame(string gameId);

    Task SetCompletedGameData(string gameId, string content);
}
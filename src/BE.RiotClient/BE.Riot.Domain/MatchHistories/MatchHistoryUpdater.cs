namespace BE.Riot.MatchHistories;

public interface IMatchHistoryUpdater
{
    Task<ISet<string>> UpdateSinceAsync(string puuId, DateTimeOffset since, CancellationToken cancellationToken);
    Task<ISet<string>> UpdateSinceLatestStoredAsync(string puuId, CancellationToken cancellationToken);
}

public sealed class MatchHistoryUpdater : IMatchHistoryUpdater
{
    private readonly IRiotClient _riotClient;
    private readonly ISummonerMatchHistoryIdsGateway _gateway;
    private const int PageSize = 100;

    public MatchHistoryUpdater(IRiotClient riotClient, ISummonerMatchHistoryIdsGateway gateway)
    {
        _riotClient = riotClient;
        _gateway = gateway;
    }

    // Ab einem gegebenen Zeitpunkt alle Seiten laden, bis keine IDs mehr kommen.
    public async Task<ISet<string>> UpdateSinceAsync(string puuId, DateTimeOffset since, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(puuId);

        var totalNew = 0;
        var start = 0;
        var seen = new HashSet<string>();
        int iterations = 0;

        HashSet<string> result = new ();
        while (!cancellationToken.IsCancellationRequested && iterations < 180)
        {
            iterations++;
            var batch = await _riotClient.GetLatestMatchIdsByPuuId(
                puuId,
                count: PageSize,
                start: start,
                startTime: since,
                endTime: null,
                queue: null,
                type: null);

            if (batch is null || batch.Count == 0)
            {
                break;
            }

            // Persistiere jede ID (Deduplikation zur Sicherheit)
            foreach (var matchId in batch)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var idStr = matchId.ToString();
                if (!seen.Add(idStr))
                {
                  
                    continue;
                }
                var match = await _riotClient.GetMatchById(idStr);
                result.Add(idStr);

                totalNew++;
            }

            // Nächste Seite
            start += batch.Count;
        }

        return result;
    }

    // Ermittelt den Startzeitpunkt aus dem zuletzt gespeicherten Match und lädt dann weiter.
    public async Task<ISet<string>> UpdateSinceLatestStoredAsync(string puuId, CancellationToken cancellationToken)
    {
        var latest = await _gateway.LatestMatchDateTime(puuId, cancellationToken);
        var since = latest ?? DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(180));
        return await UpdateSinceAsync(puuId, since, cancellationToken);
    }
}
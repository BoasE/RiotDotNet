namespace BE.Riot.Console.Infrastructure;

using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using BE.Riot.Console.Models;

public static class ChampSelectHelper
{
    public static HashSet<int> ComputeBanSet(ChampSelectSession? s)
    {
        var set = new HashSet<int>();
        if (s?.Actions == null) return set;

        foreach (var turn in s.Actions)
        foreach (var a in turn)
            if (EqualsIgnore(a.Type, "ban") && a.ChampionId != 0)
                set.Add(a.ChampionId);

        return set;
    }

    public static string? ComputeSessionFingerprint(ChampSelectSession? s)
    {
        using var sha = SHA256.Create();
        var sb = new StringBuilder();
        if (s == null) { sb.Append("NULL"); }
        else
        {
            sb.Append(s.Timer?.Phase ?? "none").Append('|');
            sb.Append(s.LocalPlayerCellId).Append('|');
            sb.Append(s.MyTeamIntentChampionId?.ToString() ?? "_").Append('|');
            sb.Append(s.MySelectionChampionId?.ToString() ?? "_").Append('|');
            sb.Append(s.MyLockedChampionId?.ToString() ?? "_").Append('|');
            foreach (var p in s.MyTeam.OrderBy(p => p.CellId))
                sb.Append(p.CellId).Append(':').Append(p.ChampionId).Append(';');

            var bans = ComputeBanSet(s).OrderBy(x => x);
            sb.Append('|').Append(string.Join(',', bans));
        }

        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString())));
    }

    public static bool EqualsIgnore(this string? a, string? b) =>
        string.Equals(a ?? "", b ?? "", StringComparison.OrdinalIgnoreCase);
}
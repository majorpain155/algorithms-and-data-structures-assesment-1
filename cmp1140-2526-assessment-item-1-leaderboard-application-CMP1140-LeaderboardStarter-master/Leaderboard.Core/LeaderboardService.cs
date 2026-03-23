namespace Leaderboard.Core;

/// <summary>
/// Operations used by the app (and the tests).
///
/// The starter version is inefficient for ranking operations:
/// it sorts all players every time you ask for Top K or Rank.
/// </summary>
public sealed class LeaderboardService
{
    private readonly IPlayerCollection _players;

    public LeaderboardService(IPlayerCollection players)
    {
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }
        _players = players;
    }

    public int PlayerCount
    {
        get { return _players.Count; }
    }

    public void AddPlayer(string id, string displayName)
    {
        AddPlayer(id, displayName, 0);
    }

    public void AddPlayer(string id, string displayName, int startingScore)
    {
        Player p = new Player(id, displayName, startingScore);
        _players.Add(p);
    }

    public Player GetPlayerOrThrow(string id)
    {
        Player p;
        bool found = _players.TryGetById(id, out p);
        if (!found || p == null)
        {
            throw new PlayerNotFoundException(id);
        }
        return p;
    }

    public void AddScore(string id, int delta)
    {
        Player p = GetPlayerOrThrow(id);
        p.AddScore(delta);
    }

    public List<Player> GetTopK(int k)
    {
        if (k < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        // Inefficient; sorts all players on every call.
        List<Player> ordered = GetAllSorted();

        List<Player> result = new List<Player>();
        int count = ordered.Count;
        if (k > count)
        {
            k = count;
        }

        for (int i = 0; i < k; i++)
        {
            result.Add(ordered[i]);
        }

        return result;
    }

    /// <summary>
    /// Returns a 1-based rank. Highest score is rank 1.
    /// Tie-break rule (for predictable results): if scores are equal, smaller Id comes first.
    /// </summary>
    public int GetRank(string id)
    {
        // Inefficient; sorts players on every call.
        List<Player> ordered = GetAllSorted();

        for (int i = 0; i < ordered.Count; i++)
        {
            if (string.Equals(ordered[i].Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return i + 1;
            }
        }

        throw new PlayerNotFoundException(id);
    }

    private List<Player> GetAllSorted()
    {
        // Make a snapshot list and sort it.
        List<Player> list = new List<Player>(_players.GetAll());

        list.Sort(new PlayerRankingComparer());
        return list;
    }

    private sealed class PlayerRankingComparer : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            // Higher score first.
            if (x.Score > y.Score) return -1;
            if (x.Score < y.Score) return 1;

            // If scores tie, smaller Id first (ordinal).
            return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
        }
    }
}

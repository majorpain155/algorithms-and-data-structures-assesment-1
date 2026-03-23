using System.Collections.ObjectModel;

namespace Leaderboard.Core;

/// <summary>
/// Simple storage for players.
///
/// - Uses a List<Player>
/// - Checks for duplicate ids by scanning the whole list
/// - Finds players by scanning the whole list
///
/// </summary>
public sealed class NaivePlayerCollection : IPlayerCollection
{
    private readonly List<Player> _players;

    public NaivePlayerCollection()
    {
        _players = new List<Player>();
    }

    public int Count
    {
        get { return _players.Count; }
    }

    public void Add(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        // Duplicate check (slow): scan every existing player.
        for (int i = 0; i < _players.Count; i++)
        {
            if (string.Equals(_players[i].Id, player.Id, StringComparison.OrdinalIgnoreCase))
            {
                throw new DuplicatePlayerIdException(player.Id);
            }
        }

        _players.Add(player);
    }

    public bool TryGetById(string playerId, out Player player)
    {
        if (playerId == null)
        {
            throw new ArgumentNullException(nameof(playerId));
        }

        // Lookup (slow): scan every existing player.
        for (int i = 0; i < _players.Count; i++)
        {
            if (string.Equals(_players[i].Id, playerId, StringComparison.OrdinalIgnoreCase))
            {
                player = _players[i];
                return true;
            }
        }

        player = null;
        return false;
    }

    public IEnumerable<Player> GetAll()
    {
        // Return a read-only shallow copy of the player list.
        return new ReadOnlyCollection<Player>(new List<Player>(_players));
    }

    public void ReplaceAll(IEnumerable<Player> players)
    {
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        _players.Clear();
        foreach (Player p in players)
        {
            _players.Add(p);
        }
    }
}
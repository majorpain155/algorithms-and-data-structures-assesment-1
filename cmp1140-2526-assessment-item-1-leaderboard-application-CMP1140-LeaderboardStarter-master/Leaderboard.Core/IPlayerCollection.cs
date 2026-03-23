namespace Leaderboard.Core;

/// <summary>
/// A simple abstraction for storing and finding players.
/// You can think of this as the "players collection" used by the leaderboard.
/// </summary>
public interface IPlayerCollection
{
    int Count { get; }

    /// <summary>
    /// Adds a new player. Should throw DuplicatePlayerIdException if the id already exists.
    /// </summary>
    void Add(Player player);

    /// <summary>
    /// Attempts to find a player by id.
    /// </summary>
    bool TryGetById(string playerId, out Player player);

    /// <summary>
    /// Returns a snapshot enumeration of all players.
    /// </summary>
    IEnumerable<Player> GetAll();

    /// <summary>
    /// Replaces everything in the collection with the provided players.
    /// </summary>
    void ReplaceAll(IEnumerable<Player> players);
}

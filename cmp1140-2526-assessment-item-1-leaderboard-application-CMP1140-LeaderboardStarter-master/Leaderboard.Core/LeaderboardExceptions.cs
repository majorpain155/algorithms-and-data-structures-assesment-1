namespace Leaderboard.Core;

public sealed class DuplicatePlayerIdException : Exception
{
    public DuplicatePlayerIdException(string playerId)
        : base($"Player id already exists: '{playerId}'.")
    {
        PlayerId = playerId;
    }

    public string PlayerId { get; }
}

public sealed class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException(string playerId)
        : base($"Player not found: '{playerId}'.")
    {
        PlayerId = playerId;
    }

    public string PlayerId { get; }
}

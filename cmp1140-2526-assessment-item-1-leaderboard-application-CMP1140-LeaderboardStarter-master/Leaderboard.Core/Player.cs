namespace Leaderboard.Core;

/// <summary>
/// Represents a single player in the leaderboard.
/// </summary>
public sealed class Player
{
    public Player(string id, string displayName, int score)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (displayName == null)
        {
            throw new ArgumentNullException(nameof(displayName));
        }

        Id = id;
        DisplayName = displayName;
        Score = score;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Stable unique identifier (e.g., username or GUID string).
    /// </summary>
    public string Id { get; }

    public string DisplayName { get; set; }

    public int Score { get; private set; }

    public DateTime LastUpdatedUtc { get; private set; }

    public void AddScore(int delta)
    {
        Score += delta;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    public void SetScore(int newScore)
    {
        Score = newScore;
        LastUpdatedUtc = DateTime.UtcNow;
    }
}

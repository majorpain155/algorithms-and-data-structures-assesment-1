using Leaderboard.Core;
using Xunit;

namespace Leaderboard.Tests;

public class CsvLeaderboardStoreTests
{
    [Fact]
    public void Save_then_load_round_trips_players()
    {
        List<Player> players = new List<Player>
        {
            new Player("id1", "Name 1", 10),
            new Player("id2", "Name, Two", 20),
            new Player("id3", "Name \"Three\"", 30),
        };

        var tempFile = Path.GetTempFileName();
        try
        {
            CsvLeaderboardStore.Save(tempFile, players);
            List<Player> loaded = CsvLeaderboardStore.Load(tempFile);

            Assert.Equal(players.Count, loaded.Count);
            for (int i = 0; i < players.Count; i++)
            {
                Assert.Equal(players[i].Id, loaded[i].Id);
                Assert.Equal(players[i].DisplayName, loaded[i].DisplayName);
                Assert.Equal(players[i].Score, loaded[i].Score);
            }
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }
}

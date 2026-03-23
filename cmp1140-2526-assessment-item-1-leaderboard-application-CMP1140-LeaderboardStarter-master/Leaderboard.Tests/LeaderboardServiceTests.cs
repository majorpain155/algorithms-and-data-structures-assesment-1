using Leaderboard.Core;
using Xunit;

namespace Leaderboard.Tests;

public class LeaderboardServiceTests
{
    private static LeaderboardService CreateService()
    {
        NaivePlayerCollection repo = new NaivePlayerCollection();
        LeaderboardService service = new LeaderboardService(repo);

        // Small deterministic set
        service.AddPlayer("a", "A", 10);
        service.AddPlayer("b", "B", 20);
        service.AddPlayer("c", "C", 20);
        service.AddPlayer("d", "D", 0);
        return service;
    }

    [Fact]
    public void AddPlayer_duplicate_id_throws()
    {
        NaivePlayerCollection repo = new NaivePlayerCollection();
        LeaderboardService service = new LeaderboardService(repo);
        service.AddPlayer("x", "X", 1);

        Assert.Throws<DuplicatePlayerIdException>(() => service.AddPlayer("x", "X2", 2));
    }

    [Fact]
    public void GetPlayerOrThrow_missing_throws()
    {
        var service = CreateService();
        Assert.Throws<PlayerNotFoundException>(() => service.GetPlayerOrThrow("missing"));
    }

    [Fact]
    public void TopK_returns_sorted_by_score_then_id()
    {
        LeaderboardService service = CreateService();
        List<Player> top = service.GetTopK(4);

        string[] ids = new string[top.Count];
        for (int i = 0; i < top.Count; i++)
        {
            ids[i] = top[i].Id;
        }

        Assert.Equal(new[] { "b", "c", "a", "d" }, ids);
    }

    [Fact]
    public void Rank_is_one_based_and_deterministic_under_ties()
    {
        LeaderboardService service = CreateService();

        // b and c tie on score (20). Tie-break is Id ordinal.
        Assert.Equal(1, service.GetRank("b"));
        Assert.Equal(2, service.GetRank("c"));
        Assert.Equal(3, service.GetRank("a"));
        Assert.Equal(4, service.GetRank("d"));
    }

    [Fact]
    public void Updating_score_affects_rank_and_topk()
    {
        LeaderboardService service = CreateService();
        service.AddScore("d", 25);

        Assert.Equal(1, service.GetRank("d"));
        List<Player> top2 = service.GetTopK(2);
        Assert.Equal("d", top2[0].Id);
        Assert.Equal("b", top2[1].Id);
    }

    [Fact]
    public void TopK_handles_k_greater_than_count()
    {
        var service = CreateService();
        var top = service.GetTopK(99);
        Assert.Equal(4, top.Count);
    }

    [Fact]
    public void TopK_handles_k_zero()
    {
        var service = CreateService();
        var top = service.GetTopK(0);
        Assert.Empty(top);
    }

    [Fact]
    public void TopK_negative_k_throws()
    {
        var service = CreateService();
        Assert.Throws<ArgumentOutOfRangeException>(() => service.GetTopK(-1));
    }

    [Fact]
    public void Rank_missing_id_throws()
    {
        var service = CreateService();
        Assert.Throws<PlayerNotFoundException>(() => service.GetRank("missing"));
    }

    [Fact]
    public void AddScore_missing_id_throws()
    {
        var service = CreateService();
        Assert.Throws<PlayerNotFoundException>(() => service.AddScore("missing", 1));
    }

    [Fact]
    public void AddPlayer_uses_starting_score()
    {
        NaivePlayerCollection repo = new NaivePlayerCollection();
        LeaderboardService service = new LeaderboardService(repo);
        service.AddPlayer("x", "X", 42);

        Player p = service.GetPlayerOrThrow("x");
        Assert.Equal(42, p.Score);
    }
}

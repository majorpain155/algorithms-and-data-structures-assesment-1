using System.Diagnostics;
using Leaderboard.Core;

namespace Leaderboard.App;

/// <summary>
/// Simple console application.
///
/// You are not assessed on the user interface. It is intentionally basic.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        NaivePlayerCollection playerCollection = new NaivePlayerCollection();
        LeaderboardService service = new LeaderboardService(playerCollection);

        Console.WriteLine("Leaderboard app (starter)");
        Console.WriteLine("Type 'help' for commands.");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }

            List<string> parts = SplitArgs(input);
            if (parts.Count == 0)
            {
                continue;
            }

            string cmd = parts[0].ToLowerInvariant();

            try
            {
                if (cmd == "help")
                {
                    PrintHelp();
                }
                else if (cmd == "exit" || cmd == "quit")
                {
                    return;
                }
                else if (cmd == "load")
                {
                    RequireArgs(parts, 2, "load <path>");
                    List<Player> players = CsvLeaderboardStore.Load(parts[1]);
                    playerCollection.ReplaceAll(players);
                    Console.WriteLine("Loaded " + service.PlayerCount + " players.");
                }
                else if (cmd == "save")
                {
                    RequireArgs(parts, 2, "save <path>");
                    CsvLeaderboardStore.Save(parts[1], playerCollection.GetAll());
                    Console.WriteLine("Saved.");
                }
                else if (cmd == "add")
                {
                    RequireArgs(parts, 3, "add <id> <displayName> [startingScore]");

                    int startingScore = 0;
                    if (parts.Count >= 4)
                    {
                        startingScore = int.Parse(parts[3]);
                    }

                    service.AddPlayer(parts[1], parts[2], startingScore);
                    Console.WriteLine("Added.");
                }
                else if (cmd == "score")
                {
                    RequireArgs(parts, 3, "score <id> <delta>");
                    service.AddScore(parts[1], int.Parse(parts[2]));
                    Console.WriteLine("Updated.");
                }
                else if (cmd == "top")
                {
                    int k = 10;
                    if (parts.Count >= 2)
                    {
                        k = int.Parse(parts[1]);
                    }

                    List<Player> top = service.GetTopK(k);
                    for (int i = 0; i < top.Count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + top[i].Id + " | " + top[i].DisplayName + " | " + top[i].Score);
                    }
                }
                else if (cmd == "rank")
                {
                    RequireArgs(parts, 2, "rank <id>");
                    Console.WriteLine("Rank: " + service.GetRank(parts[1]));
                }
                else if (cmd == "show")
                {
                    RequireArgs(parts, 2, "show <id>");
                    Player p = service.GetPlayerOrThrow(parts[1]);
                    Console.WriteLine(p.Id + " | " + p.DisplayName + " | score=" + p.Score);
                }
                else if (cmd == "bench")
                {
                    RunBenchmark(service);
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  load <path>                          Load CSV (with header: id,displayName,score)");
        Console.WriteLine("  save <path>                          Save CSV");
        Console.WriteLine("  add <id> <displayName> [score]       Add a player (displayName has no spaces in starter)");
        Console.WriteLine("  score <id> <delta>                   Add delta to a player's score");
        Console.WriteLine("  top [k]                              Show top k players (default 10)");
        Console.WriteLine("  rank <id>                            Show player's 1-based rank");
        Console.WriteLine("  show <id>                            Show player record");
        Console.WriteLine("  bench                                Run a small timing harness");
        Console.WriteLine("  quit                                 Exit");
    }

    private static void RequireArgs(List<string> parts, int min, string usage)
    {
        if (parts.Count < min)
        {
            throw new ArgumentException("Usage: " + usage);
        }
    }

    private static List<string> SplitArgs(string input)
    {
        // Simple split by spaces. (Starter app does not support quoted args.)
        string[] raw = input.Split(' ');
        List<string> parts = new List<string>();

        for (int i = 0; i < raw.Length; i++)
        {
            string item = raw[i].Trim();
            if (item.Length > 0)
            {
                parts.Add(item);
            }
        }

        return parts;
    }

    private static void RunBenchmark(LeaderboardService service)
    {
        if (service.PlayerCount == 0)
        {
            Console.WriteLine("Load some data first (e.g., load data/players_large.csv).");
            return;
        }

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 100; i++)
        {
            service.GetTopK(10);
        }
        sw.Stop();

        Console.WriteLine("Top 10 x100: " + sw.ElapsedMilliseconds + " ms");
    }
}

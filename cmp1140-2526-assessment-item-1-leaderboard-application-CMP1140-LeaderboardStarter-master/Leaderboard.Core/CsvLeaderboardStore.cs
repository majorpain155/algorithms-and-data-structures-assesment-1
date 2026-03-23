using System.Globalization;
using System.Text;
using CsvHelper;

namespace Leaderboard.Core;

/// <summary>
/// CSV loader/saver for the leaderboard.
///
/// Format: id,displayName,score
///
/// This uses the CsvHelper library so we do not have to write our own CSV parser.
/// </summary>
public static class CsvLeaderboardStore
{
    public static List<Player> Load(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("CSV file not found", path);
        }

        List<Player> players = new List<Player>();

        using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            // If the file has a header row, this will read it.
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                string id = csv.GetField<string>(0);
                string name = csv.GetField<string>(1);
                int score = csv.GetField<int>(2);

                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new FormatException("CSV contains a blank id.");
                }

                players.Add(new Player(id, name, score));
            }
        }

        return players;
    }

    public static void Save(string path, IEnumerable<Player> players)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            // Header
            csv.WriteField("id");
            csv.WriteField("displayName");
            csv.WriteField("score");
            csv.NextRecord();

            foreach (Player p in players)
            {
                csv.WriteField(p.Id);
                csv.WriteField(p.DisplayName);
                csv.WriteField(p.Score);
                csv.NextRecord();
            }
        }
    }
}

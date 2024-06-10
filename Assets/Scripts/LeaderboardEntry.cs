using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

[Serializable]
public class Leaderboard
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    public void AddEntry(LeaderboardEntry entry)
    {
        entries.Add(entry);
        entries.Sort((x, y) => y.score.CompareTo(x.score));
    }
}

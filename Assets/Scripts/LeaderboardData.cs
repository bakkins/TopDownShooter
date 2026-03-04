using System;
using System.Collections.Generic;

[Serializable]
public class HighScoreEntry {
    public string name;
    public int score;
}

[Serializable]
public class LeaderboardData {
    public List<HighScoreEntry> entries = new List<HighScoreEntry>();
}
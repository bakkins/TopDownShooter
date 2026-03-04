using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject MainMenuPanel; // The Settings Sub-Panel
    public GameObject leaderboardPanel;

    private string filePath;
    public LeaderboardData data = new LeaderboardData();

    void Awake() {
        // Saves to C:/Users/Name/AppData/LocalLow/YourCompany/YourGame
        filePath = Application.persistentDataPath + "/leaderboard.json";
        LoadData();
    }

    public void AddEntry(string name, int score) {
        data.entries.Add(new HighScoreEntry { name = name, score = score });
        
        // Sort by highest score and keep only top 10
        data.entries = data.entries.OrderByDescending(s => s.score).Take(10).ToList();
        
        SaveData();
    }

    public void SaveData() {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadData() {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<LeaderboardData>(json);
        }
    }

        public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
}
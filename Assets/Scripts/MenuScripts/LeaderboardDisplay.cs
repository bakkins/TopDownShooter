using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    public LeaderboardManager leaderboardManager;
    public TMP_Text[] entryTexts; 

    void OnEnable()
    {
        if (leaderboardManager != null)
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        leaderboardManager.LoadData();
        var scores = leaderboardManager.data.entries;

        for (int i = 0; i < entryTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                entryTexts[i].text = (i + 1) + ". " + scores[i].name + " - " + scores[i].score;
            }
            else
            {
                entryTexts[i].text = (i + 1) + ". --- 000";
            }
        }
    }
}
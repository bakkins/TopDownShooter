using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathPanelManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathPanel;
    public TMP_Text finalScoreText;
    public TMP_InputField nameInputField;
    public LeaderboardManager leaderboardManager;

    private int scoreAtDeath;

    void Start()
    {
        // Ensure the panel is hidden when the level starts
        deathPanel.SetActive(false);
        
        // Force the input field to only allow 3 characters
        nameInputField.characterLimit = 3;
    }

    // Call this function from your Player script when they die
    public void ToggleDeathPanel(int finalScore)
    {
        scoreAtDeath = finalScore;
        finalScoreText.text = "FINAL SCORE: " + scoreAtDeath.ToString();
        
        deathPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        
        // Focus the input field so the player can type immediately
        nameInputField.ActivateInputField();
    }

    public void SubmitAndReturn()
    {
        string playerName = nameInputField.text.ToUpper();

        // Only allow submission if they entered 3 characters
        if (playerName.Length == 3)
        {
            leaderboardManager.AddEntry(playerName, scoreAtDeath);
            
            // Resume time before leaving the scene!
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {
            // Optional: Shake the input field or change color to show error
            Debug.Log("Please enter exactly 3 characters.");
        }
    }
}
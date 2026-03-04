using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pauseMenuUI; // The main Pause Panel
    public GameObject settingsPanel; // The Settings Sub-Panel

    void Update()
    {
        // Toggle pause when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Resumes the game world
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freezes the game world
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // MUST reset time or the main menu will be frozen
        SceneManager.LoadScene("MainMenuScene");
    }

        public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time first!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
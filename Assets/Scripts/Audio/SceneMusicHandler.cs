using UnityEngine;

public class SceneMusicHandler : MonoBehaviour
{
    public enum SceneType { MainMenu, Game }
    public SceneType currentScene;

    void Start()
    {
        // Give SoundManager a moment to exist if coming from menu
        Invoke(nameof(PlaySceneMusic), 0.1f);
    }

    void PlaySceneMusic()
    {
        if (SoundManager.instance != null)
        {
            if (currentScene == SceneType.MainMenu)
            {
                SoundManager.instance.PlayMusic(SoundManager.instance.mainMenuMusic);
            }
            else if (currentScene == SceneType.Game)
            {
                SoundManager.instance.PlayMusic(SoundManager.instance.gameMusic);
            }
        }
    }
}
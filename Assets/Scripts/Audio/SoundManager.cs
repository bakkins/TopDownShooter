using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;

    [Header("SFX Clips")]
    public AudioClip playerDamage;
    public AudioClip playerShoot;
    public AudioClip enemyShoot;
    public AudioClip grenadeExplosion;
    public AudioClip enemyDamage;
    public AudioClip bossShoot;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Exit so we don't try to assign sources to a destroyed object
        }

        // --- KEY FIX ---
        // Find the AudioSource components on THIS GameObject
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource = sources[1];
        }
        else
        {
            Debug.LogError("SoundManager needs at least 2 AudioSource components!");
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            // Now it uses the volume scale we passed in
            sfxSource.PlayOneShot(clip, volume);
    }
        }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

        // Add this inside SoundManager.cs
    public void SetMusicVolume(float volume)
    {
        // Volume is a float between 0.0 and 1.0
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
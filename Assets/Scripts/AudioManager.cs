
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Effects")]
    public AudioClip crystalSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip footstepSound;
    public AudioClip jumpSound;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public AudioClip menuMusic;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource musicSource;
    private AudioSource soundSource;

    private float musicVolume;
    private float sfxVolume;

    private const string MusicKey = "MusicVolumeSaved";
    private const string SFXKey = "SFXVolumeSaved";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();

        musicSource = sources.Length > 0
            ? sources[0]
            : gameObject.AddComponent<AudioSource>();

        soundSource = sources.Length > 1
            ? sources[1]
            : gameObject.AddComponent<AudioSource>();

        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;

        soundSource.playOnAwake = false;
        soundSource.loop = false;
        soundSource.spatialBlend = 0f;

        musicSource.mute = false;
        soundSource.mute = false;

        if (audioMixer != null)
        {
            AudioMixerGroup[] musicGroups =
                audioMixer.FindMatchingGroups("Music");

            AudioMixerGroup[] sfxGroups =
                audioMixer.FindMatchingGroups("SFX");

            if (musicGroups.Length > 0)
                musicSource.outputAudioMixerGroup = musicGroups[0];

            if (sfxGroups.Length > 0)
                soundSource.outputAudioMixerGroup = sfxGroups[0];
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetMusicVolume(
            PlayerPrefs.GetFloat(MusicKey, 0.5f)
        );

        SetSFXVolume(
            PlayerPrefs.GetFloat(SFXKey, 0.8f)
        );

        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip selectedMusic = null;

        if (sceneName == "MainMenu")
        {
            selectedMusic = menuMusic;
        }
        else if (sceneName == "Caves")
        {
            selectedMusic = backgroundMusic;
        }

        if (selectedMusic == null)
        {
            musicSource.Stop();
            musicSource.clip = null;
            return;
        }

        if (musicSource.clip != selectedMusic)
        {
            musicSource.Stop();
            musicSource.clip = selectedMusic;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource == null)
            return;

        if (musicSource.clip == null)
        {
            PlayMusicForScene(SceneManager.GetActiveScene().name);
            return;
        }

        musicSource.UnPause();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void RestartMusic()
    {
        if (musicSource != null)
            musicSource.Stop();

        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    public void PlayCrystal()
    {
        PlaySound(crystalSound);
    }

    public void PlayWin()
    {
        PauseMusic();
        PlaySound(winSound);
    }

    public void PlayLose()
    {
        StopMusic();
        PlaySound(loseSound);
    }

    public void PlayFootstep()
    {
        PlaySound(footstepSound);
    }

    public void PlayJump()
    {
        PlaySound(jumpSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && soundSource != null)
        {
            soundSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.Save();

        if (audioMixer != null &&
            musicSource.outputAudioMixerGroup != null)
        {
            musicSource.volume = 1f;

            float adjustedVolume = musicVolume * 0.2f;

            float db = adjustedVolume <= 0.001f
                ? -80f
                : Mathf.Log10(adjustedVolume) * 20f;

            audioMixer.SetFloat("MusicVolume", db);
        }
        else
        {
            musicSource.volume = musicVolume * 0.2f;
        }
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat(SFXKey, sfxVolume);
        PlayerPrefs.Save();

        if (audioMixer != null &&
            soundSource.outputAudioMixerGroup != null)
        {
            soundSource.volume = 1f;

            float db = sfxVolume <= 0.001f
                ? -80f
                : Mathf.Log10(sfxVolume) * 20f;

            audioMixer.SetFloat("SFXVolume", db);
        }
        else
        {
            soundSource.volume = sfxVolume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetVolume(float value)
    {
        SetMusicVolume(value);
        SetSFXVolume(value);
    }

    public void SetVolume(string parameter, float value)
    {
        if (parameter == "MusicVolume")
        {
            SetMusicVolume(value);
        }
        else if (parameter == "SFXVolume")
        {
            SetSFXVolume(value);
        }
        else
        {
            SetVolume(value);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }
}

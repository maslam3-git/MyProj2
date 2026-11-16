
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
            return;

        musicSlider.SetValueWithoutNotify(
            AudioManager.Instance.GetMusicVolume()
        );

        sfxSlider.SetValueWithoutNotify(
            AudioManager.Instance.GetSFXVolume()
        );

        musicSlider.onValueChanged.AddListener(ChangeMusic);
        sfxSlider.onValueChanged.AddListener(ChangeSFX);
    }

    public void ChangeMusic(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void ChangeSFX(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(ChangeMusic);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(ChangeSFX);
    }
}

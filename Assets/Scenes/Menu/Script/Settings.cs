using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Toggle postProcessingToggle;
    public Slider volumeSlider;
    public AudioMixer mixer;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
        SetTargetFPS(120);

        postProcessingToggle.onValueChanged.AddListener(TogglePostProcessing);
        postProcessingToggle.isOn = PlayerPrefs.GetInt("PostProcessingEnabled", 1) == 1;
        TogglePostProcessing(postProcessingToggle.isOn);
    }

    public void SetMusicVolume()
    {
        float volume = volumeSlider.value;
        mixer.SetFloat("music", +Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetTargetFPS(int targetFps)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }

    private void TogglePostProcessing(bool isEnabled)
    {
        postProcessVolume.enabled = isEnabled;

        // Сохранение состояния Toggle
        PlayerPrefs.SetInt("PostProcessingEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OpenMyWeb()
    {
        Application.OpenURL("https://renothingg.github.io/ReNothingg/");
    }

    public void ExitGame() => Application.Quit();
}
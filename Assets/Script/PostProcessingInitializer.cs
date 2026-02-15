using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingInitializer : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;

    private void Start()
    {
        // Загрузка состояния Toggle при запуске сцены
        bool isPostProcessingEnabled = PlayerPrefs.GetInt("PostProcessingEnabled", 1) == 1;
        postProcessVolume.enabled = isPostProcessingEnabled;
    }
}

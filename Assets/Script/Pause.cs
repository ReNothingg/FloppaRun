using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button open;
    public Button close;
    public GameObject panelPause;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        panelPause.SetActive(false);
        open.onClick.AddListener(TogglePause);
        close.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        panelPause.SetActive(isPaused);

        Debug.Log(isPaused ? "Paused" : "Unpaused");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    public Button button;
    public string scene;
    void Start() { 
        button.onClick.AddListener(Click);
        Time.timeScale = 1.0f;
    }
    void Click() { SceneManager.LoadScene(scene); }
}

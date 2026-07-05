using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    public Button button;
    public string scene;
    private void Start() {
        button.onClick.AddListener(Click);
        Time.timeScale = 1.0f;
    }
    
    private void Click() { 
        SceneManager.LoadScene(scene); 
        Time.timeScale = 1; 
    }
}

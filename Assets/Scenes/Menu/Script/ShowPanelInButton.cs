using UnityEngine;
using UnityEngine.UI;
public class ShowPanelInButton : MonoBehaviour {
    public Button button;
    public GameObject Object;
    public bool SetActivet = false;
    void Start() { button.onClick.AddListener(Click); }
    void Click() { Object.SetActive(SetActivet); }
}

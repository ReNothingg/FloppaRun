using UnityEngine;
using UnityEngine.UI;

public class ShowPanelInButton : MonoBehaviour 
{
    public Button button;
    public GameObject Object;
    public bool SetActivet = false;

    private void Start() => button.onClick.AddListener(Click);
    private void Click() => Object.SetActive(SetActivet);
}

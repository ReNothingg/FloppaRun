using UnityEngine;

public class TogglePanelBut : MonoBehaviour
{
    public GameObject panel;
    private bool isActive = false;

    public void OnButtonClick()
    {
        isActive = !isActive;
        panel.SetActive(isActive);
    }
}

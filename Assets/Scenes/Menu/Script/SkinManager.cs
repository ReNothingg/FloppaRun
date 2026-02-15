using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public Sprite[] skins;
    public Button[] skinButtons;
    private int selectedSkinIndex;

    private void Awake()
    {
        selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        ApplySkin();

        for (int i = 0; i < skinButtons.Length; i++)
        {
            int index = i;
            skinButtons[i].onClick.AddListener(() => ChangeSkin(index));
        }
    }

    public void ChangeSkin(int index)
    {
        if (index >= 0 && index < skins.Length)
        {
            selectedSkinIndex = index;
            PlayerPrefs.SetInt("SelectedSkin", selectedSkinIndex);
            ApplySkin();
        }
    }

    private void ApplySkin()
    {
        playerSpriteRenderer.sprite = skins[selectedSkinIndex];
    }
}

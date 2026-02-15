using UnityEngine;
using UnityEngine.UI;

public class BuySkin : MonoBehaviour
{
    public string objectName;
    public int price, isBuyed;
    public Text priceTxt;

    private Button block;

    void Awake()
    {
        isBuyedAwake();
    }

    void isBuyedAwake()
    {
        isBuyed = PlayerPrefs.GetInt(objectName + "Acces");
        block = GetComponent<Button>();
        priceTxt.text = price.ToString();

        if (isBuyed == 1)
        {
            block.interactable = true;
            priceTxt.text = "Buyed";
        }
    }

    public void OnButtonDown()
    {
        int coins = PlayerPrefs.GetInt("Money");

        if (isBuyed == 0)
        {
            if (coins >= price)
            {
                PlayerPrefs.SetInt(objectName + "Acces", 1);
                PlayerPrefs.SetInt("Money", coins - price);
                isBuyedAwake();
            }
        }
    }
}

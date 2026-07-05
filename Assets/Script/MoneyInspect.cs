using System;
using UnityEngine;
using UnityEngine.UI;

public class MoneyInspect : MonoBehaviour
{
    public MoneySCOB money;
    public Text text;
    public String stertText;

    public bool isDwetochki = true;

    private void Update() {
        string separator = isDwetochki ? ": " : string.Empty;
        text.text = stertText + separator + money.money.ToString();
    }
}

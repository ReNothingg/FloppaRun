using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyInspect : MonoBehaviour
{
    public MoneySCOB money;
    public Text text;
    public String stertText;
    void Update() { text.text = (stertText + ": " + Convert.ToString(money.money)); }
}

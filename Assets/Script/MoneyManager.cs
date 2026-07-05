using UnityEngine;
public class MoneyManager : MonoBehaviour
{
    public MoneySCOB money;
    private void Start() => money.money = PlayerPrefs.GetInt("Money");
    private void Update() => PlayerPrefs.SetInt("Money", money.money);
}
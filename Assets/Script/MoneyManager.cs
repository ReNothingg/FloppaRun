using UnityEngine;
public class MoneyManager : MonoBehaviour
{
    public MoneySCOB money;
    private void Start() => money.money = PlayerPrefs.GetInt("Money");
    void Update() => PlayerPrefs.SetInt("Money", money.money);
}
using UnityEngine;

public class Pelmenik : MonoBehaviour
{
    public MoneySCOB money;
    public int addMoney;
    private void OnTriggerEnter2D(Collider2D collision) { if(collision.tag == "Player") { money.money += addMoney; Destroy(gameObject); } }
}

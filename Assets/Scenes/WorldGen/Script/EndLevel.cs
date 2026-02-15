using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public MoneySCOB money;
    public GameObject endPanel;
    public int bonus;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
        {
            money.money += bonus;
            collision.gameObject.GetComponent<PlayerController>().canMove = false;
            endPanel.SetActive(true);
            Destroy(collision.gameObject.GetComponent<PlayerController>());
        }
    }
}

using UnityEngine;

public class addHealthIW : MonoBehaviour
{
    private PlayerIW controller;
    public MoneySCOB money;
    void Start() { controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIW>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int add = Random.Range(4, 10);
            if (controller.health != 100) controller.health += add;
            else if (controller.health >= (100 - add)) money.money += 5;
            Destroy(gameObject);
        }
    }
}

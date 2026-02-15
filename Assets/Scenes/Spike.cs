using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damage = 10;
    public float speed;
    public bool destroy;

    public bool destroyAfterTime;
    public float destroyTime = 3;
    public bool move;

    void FixedUpdate()
    {
        if (move == true) transform.Translate(Vector2.left * speed);

        if (destroyAfterTime == true) Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>()) collision.GetComponent<PlayerController>().takeGamage(damage);
            if (collision.GetComponent<PlayerIW>()) collision.GetComponent<PlayerIW>().GetDamage(damage);
            if (collision.GetComponent<PlayerControll>()) collision.GetComponent <PlayerControll>().GetDamage(damage);

            if (destroy == true) Destroy(gameObject);
        }
    }
}

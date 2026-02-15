using UnityEngine;

public class PlayerBulled : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifeTime;
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed);
        Destroy(gameObject, lifeTime);
    }
}

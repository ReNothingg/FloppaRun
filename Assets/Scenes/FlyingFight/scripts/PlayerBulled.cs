using UnityEngine;

public class PlayerBulled : MonoBehaviour
{
    public int damage;
    public float speed, lifeTime;
    private void FixedUpdate() {
        transform.Translate(Vector3.right * speed);
        Destroy(gameObject, lifeTime);
    }
}

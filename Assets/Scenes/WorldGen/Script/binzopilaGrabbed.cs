using UnityEngine;

public class binzopilaGrabbed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spike") { Destroy(collision.gameObject); }
    }
}

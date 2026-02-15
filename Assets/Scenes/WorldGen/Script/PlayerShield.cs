using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public float shieldDuration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            StartCoroutine(Boost(player));
        }
    }

    private IEnumerator Boost(PlayerController player)
    {
        player.bronikBool = true;

        yield return new WaitForSeconds(shieldDuration);

        player.bronikBool = false;

        Destroy(gameObject);
    }
}

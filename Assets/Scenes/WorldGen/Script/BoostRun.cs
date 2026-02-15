using UnityEngine;
using System.Collections;
public class BoostRun : MonoBehaviour
{
    public float boostAmount = 2.0f;
    public float boostDuration = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(Boost(player));
            }
        }
    }

    private IEnumerator Boost(PlayerController player)
    {
        float originalSpeed = player.speed;
        player.speed += boostAmount;

        yield return new WaitForSeconds(boostDuration);

        player.speed = originalSpeed;

        Destroy(gameObject);
    }
}

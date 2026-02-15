using System.Collections;
using UnityEngine;

public class TimeSlowdownBoost : MonoBehaviour
{
    public float slowdownFactor = 0.6f;
    public float slowdownDuration = 2.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SlowTime());
        }
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSeconds(slowdownDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        Destroy(gameObject);
    }
}

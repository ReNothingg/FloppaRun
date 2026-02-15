using UnityEngine;

public class SpawnerIW : MonoBehaviour
{
    public GameObject[] gear;

    private float timeBtwSpawn;
    public float startTimeBtwSpawn = 2.0f;
    public float decreaseTime = 0.03f;
    public float minTime = 0.65f;

    private void Start()
    {
        timeBtwSpawn = startTimeBtwSpawn;
    }

    private void Update()
    {
        if (timeBtwSpawn <= 0)
        {
            int rand = Random.Range(0, gear.Length);
            Instantiate(gear[rand], transform.position, Quaternion.identity);

            if (startTimeBtwSpawn > minTime)
            {
                startTimeBtwSpawn -= decreaseTime;
                if (startTimeBtwSpawn < minTime)
                {
                    startTimeBtwSpawn = minTime;
                }
            }

            timeBtwSpawn = startTimeBtwSpawn;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}

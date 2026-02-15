using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int Yspawn;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, new Vector2(transform.position.x, Random.Range(-Yspawn, Yspawn)), Quaternion.identity);
    }
}

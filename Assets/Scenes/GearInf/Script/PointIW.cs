using UnityEngine;
using System.Collections.Generic;

public class PointIW : MonoBehaviour
{
    public GameObject gear;

    public bool isBoost;
    [System.Serializable]
    public class BoostEntry
    {
        public GameObject boostPrefab;
        [Range(0, 100)] public float weight;
    }

    [SerializeField] public List<BoostEntry> boostOptions;

    private void Start()
    {
        if(isBoost == false) Instantiate(gear, transform.position, Quaternion.identity);

        if (isBoost && boostOptions.Count > 0)
        {
            GameObject chosenBoost = GetRandomBoost();

            if (chosenBoost != null)
            {
                Instantiate(chosenBoost, transform.position, Quaternion.identity);
            }
        }
    }

    private GameObject GetRandomBoost()
    {
        float totalWeight = 0f;
        foreach (var boost in boostOptions)
        {
            totalWeight += boost.weight;
        }

        if (totalWeight <= 0f)
            return null;

        float randomValue = Random.Range(0, totalWeight);
        float currentSum = 0f;

        foreach (var boost in boostOptions)
        {
            currentSum += boost.weight;
            if (randomValue <= currentSum)
            {
                return boost.boostPrefab;
            }
        }

        return null;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Space(10), Header("Prefabs")]
    public GameObject grassPrefab;
    public GameObject grassLeftEdgePrefab;
    public GameObject grassRightEdgePrefab;
    public GameObject dirtPrefab;

    [Space(5)]
    public GameObject[] cloudPrefabs;

    [Space(5)]
    public GameObject mainJavaPrefab;
    public GameObject blockJavaPrefab;
    public GameObject UpperLakePrefab;
    public GameObject DownBlockLakePrefab;

    [Space(5)]
    public GameObject flag;

    [Space(10), Header("Level Settings")]
    public int levelLength = 600;
    [Range(1, 10)] public int groundThickness = 6;
    [Range(-20, 0)] public int javaY = -5;
    [Range(1, 10)] public int javaLayerCount = 4;

    [Space(10), Header("Distance & Feature Settings")]
    public int minDistanceForHeightChangeAttempt = 5;
    public int maxHeightVariation = 3;

    [Header("Lake Settings")]
    public int minLakeLength = 10;
    public int maxLakeLength = 20;
    public GameObject lakeWarningPrefab;
    [Range(0.1f, 0.9f)] public float lakeAttemptPositionFactor = 0.5f;

    [Header("Hole Settings")]
    public int minHoleWidth = 1;
    public int maxHoleWidth = 10;
    public GameObject lowHoleJumperPrefab;
    public GameObject mediumHoleJumperPrefab;
    public GameObject highHoleJumperPrefab;
    public GameObject holeWarningPrefab;
    public int minDistanceBetweenHoles = 50;
    [Range(0.0f, 1.0f)] public float holeSpawnChance = 0.2f;

    [Space(10), Header("Cloud Settings")]
    public float cloudHorizontalSpacing = 15f;
    public float cloudMinY = 9f;
    public float cloudMaxY = 14f;
    [Range(0.0f, 1.0f)] public float cloudSpawnChance = 0.45f;

    [Space(10), Header("Under Java Settings")]
    public float javaBlockWidth = 1f;

    [Space(10), Header("Custom Items")]
    public NewItem[] newItems;

    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();
    private Transform _generatedLevelParent;
    private const string GENERATED_LEVEL_PARENT_NAME = "GeneratedLevelContent";
    private Dictionary<Vector2Int, GameObject> _terrainBlocks = new Dictionary<Vector2Int, GameObject>();


    void Start()
    {
        CreateLevelParent();
        Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Generate();
        }
    }

    void CreateLevelParent()
    {
        if (_generatedLevelParent == null)
        {
            GameObject parentObject = GameObject.Find(GENERATED_LEVEL_PARENT_NAME);
            if (parentObject == null)
            {
                parentObject = new GameObject(GENERATED_LEVEL_PARENT_NAME);
            }
            _generatedLevelParent = parentObject.transform;
        }
    }

    void ClearLevel()
    {
        if (_generatedLevelParent == null) CreateLevelParent();

        foreach (Transform child in _generatedLevelParent)
        {
            Destroy(child.gameObject);
        }
        occupiedPositions.Clear();
        _terrainBlocks.Clear();
    }

    public void Generate()
    {
        ClearLevel();
        GenerateLevel();
        GenerateClouds();
        GenerateUnderJava();
    }

    GameObject PlaceOrReplaceBlock(Vector2Int gridPos, GameObject prefab, Quaternion rotation)
    {
        if (_terrainBlocks.TryGetValue(gridPos, out GameObject existingBlock))
        {
            Destroy(existingBlock);
        }

        if (prefab != null)
        {
            GameObject newBlock = Instantiate(prefab, (Vector2)gridPos, rotation, _generatedLevelParent);
            _terrainBlocks[gridPos] = newBlock;
            return newBlock;
        }

        _terrainBlocks.Remove(gridPos);
        return null;
    }


    void GenerateLevel()
    {
        Dictionary<NewItem, int> lastItemPositions = new Dictionary<NewItem, int>();
        foreach (var item in newItems)
        {
            lastItemPositions[item] = -item.minDistance;
        }

        int currentHeight = 0;
        int lastHeight = 0;
        int lastHolePosition = -minDistanceBetweenHoles;

        bool riverPlaced = false;
        int lakeLength = Random.Range(minLakeLength, maxLakeLength + 1);
        int lakeStartPosition = Mathf.FloorToInt(levelLength * lakeAttemptPositionFactor);

        for (int i = 0; i < levelLength; i++)
        {
            Vector2Int currentPos = new Vector2Int(i, 0);


            if (!riverPlaced && i >= lakeStartPosition && i + lakeLength < levelLength)
            {
                if (lakeWarningPrefab != null)
                    Instantiate(lakeWarningPrefab, new Vector2(i - 1, (groundThickness + currentHeight) - 0.2f), Quaternion.Euler(0, 0, -15), _generatedLevelParent);

                for (int lakeIdx = 0; lakeIdx < lakeLength; lakeIdx++)
                {
                    int riverX = i + lakeIdx;
                    Vector2Int lakeBlockPos = new Vector2Int(riverX, 0);
                    if (UpperLakePrefab != null)
                        Instantiate(UpperLakePrefab, new Vector2(riverX, (groundThickness) - 0.12f), Quaternion.identity, _generatedLevelParent);

                    for (int j = groundThickness - 1; j >= 0; j--)
                    {
                        PlaceOrReplaceBlock(new Vector2Int(riverX, j), DownBlockLakePrefab, Quaternion.identity);
                    }
                }
                i += lakeLength - 1;
                riverPlaced = true;
                lastHeight = 0;
                currentHeight = 0;
                continue;
            }

            if (i - lastHolePosition >= minDistanceBetweenHoles && Random.value < holeSpawnChance && i + minHoleWidth < levelLength - maxHoleWidth)
            {
                int holeWidth = Random.Range(minHoleWidth, maxHoleWidth + 1);

                if (holeWarningPrefab != null && holeWidth > 9)
                    Instantiate(holeWarningPrefab, new Vector2(i - 1, (groundThickness + currentHeight) - 0.2f), Quaternion.Euler(0, 0, 15), _generatedLevelParent);

                Vector2 jumperPos = new Vector2(i + 0.5f * (holeWidth - 1), (groundThickness + currentHeight) - 0.2f);
                if (holeWidth >= 10 && highHoleJumperPrefab != null) Instantiate(highHoleJumperPrefab, jumperPos, Quaternion.identity, _generatedLevelParent);
                else if (holeWidth >= 8 && mediumHoleJumperPrefab != null) Instantiate(mediumHoleJumperPrefab, jumperPos, Quaternion.identity, _generatedLevelParent);
                else if (holeWidth >= 5 && lowHoleJumperPrefab != null) Instantiate(lowHoleJumperPrefab, jumperPos, Quaternion.identity, _generatedLevelParent);

                lastHolePosition = i + holeWidth - 1;
                i += holeWidth - 1;
                lastHeight = currentHeight;
                continue;
            }

            if (i > 0 && (minDistanceForHeightChangeAttempt <= 1 || i % minDistanceForHeightChangeAttempt == 0))
            {
                int heightChange = Random.Range(-maxHeightVariation, maxHeightVariation + 1);
                currentHeight = Mathf.Clamp(currentHeight + heightChange, 0, this.maxHeightVariation);
            }

            for (int y = 0; y < groundThickness + currentHeight; y++)
            {
                GameObject prefabToPlace = (y == groundThickness + currentHeight - 1) ? grassPrefab : dirtPrefab;
                PlaceOrReplaceBlock(new Vector2Int(i, y), prefabToPlace, Quaternion.identity);
            }

            if (i > 0)
            {
                if (currentHeight > lastHeight)
                {
                    Vector2Int capPos = new Vector2Int(i, groundThickness + currentHeight - 1);
                    GameObject capPrefab = grassLeftEdgePrefab != null ? grassLeftEdgePrefab : grassPrefab;
                    PlaceOrReplaceBlock(capPos, capPrefab, Quaternion.identity);

                    int wallStartY = groundThickness + currentHeight - 2;
                    int wallEndY = groundThickness + lastHeight;
                    for (int yWall = wallStartY; yWall >= wallEndY; yWall--)
                    {
                        Vector2Int wallPos = new Vector2Int(i, yWall);
                        GameObject wallSegmentPrefab = grassPrefab;
                        PlaceOrReplaceBlock(wallPos, wallSegmentPrefab, Quaternion.Euler(0, 0, 90));
                    }
                }
                else if (currentHeight < lastHeight)
                {
                    Vector2Int capPos = new Vector2Int(i - 1, groundThickness + lastHeight - 1);
                    GameObject capPrefab = grassRightEdgePrefab != null ? grassRightEdgePrefab : grassPrefab;
                    PlaceOrReplaceBlock(capPos, capPrefab, Quaternion.identity);

                    int wallStartY = groundThickness + lastHeight - 2;
                    int wallEndY = groundThickness + currentHeight;
                    for (int yWall = wallStartY; yWall >= wallEndY; yWall--)
                    {
                        Vector2Int wallPos = new Vector2Int(i - 1, yWall);
                        GameObject wallSegmentPrefab = grassPrefab;
                        PlaceOrReplaceBlock(wallPos, wallSegmentPrefab, Quaternion.Euler(0, 0, -90));
                    }
                }
            }
            lastHeight = currentHeight;


            foreach (var item in newItems)
            {
                if (item.prefab != null && i - lastItemPositions[item] >= item.minDistance && Random.value < item.spawnChance)
                {
                    Vector2 spawnPosition = new Vector2(i, (groundThickness + currentHeight) + item.yOffset);
                    if (!occupiedPositions.Contains(spawnPosition))
                    {
                        Instantiate(item.prefab, spawnPosition, Quaternion.identity, _generatedLevelParent);
                        lastItemPositions[item] = i;
                        occupiedPositions.Add(spawnPosition);
                    }
                }
            }
        }

        if (flag != null)
        {
            flag.transform.position = new Vector2(levelLength + 5, groundThickness + lastHeight);
            if (_generatedLevelParent != null) flag.transform.SetParent(_generatedLevelParent);
        }
    }

    void GenerateClouds()
    {
        if (cloudPrefabs == null || cloudPrefabs.Length == 0 || cloudHorizontalSpacing <= 0) return;
        float currentX = 0f;
        while (currentX < levelLength)
        {
            if (Random.value < cloudSpawnChance)
            {
                GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
                if (cloudPrefab != null)
                {
                    float cloudY = Random.Range(this.maxHeightVariation + groundThickness + cloudMinY, this.maxHeightVariation + groundThickness + cloudMaxY);
                    Instantiate(cloudPrefab, new Vector3(currentX, cloudY, 0), Quaternion.identity, _generatedLevelParent);
                }
            }
            currentX += cloudHorizontalSpacing;
        }
    }

    void GenerateUnderJava()
    {
        if (mainJavaPrefab == null || blockJavaPrefab == null || javaBlockWidth <= 0) return;
        float currentX = 0f;
        while (currentX < levelLength)
        {
            Instantiate(mainJavaPrefab, new Vector3(currentX, javaY - 0.12f, 0), Quaternion.identity, _generatedLevelParent);
            for (int j = 0; j < javaLayerCount; j++)
            {
                Instantiate(blockJavaPrefab, new Vector3(currentX, javaY - (1 + j), 0), Quaternion.identity, _generatedLevelParent);
            }
            currentX += javaBlockWidth;
        }
    }
}

[System.Serializable]
public class NewItem
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance = 0.05f;
    [Tooltip("Vertical offset from the top of the ground.")]
    [Range(-2f, 5f)] public float yOffset = 0f;
    [Tooltip("Minimum horizontal distance (in blocks) from the last spawned item of this type.")]
    public int minDistance = 10;
}
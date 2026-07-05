using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
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


    private void Start() {
        CreateLevelParent();
        Generate();
    }

    private void CreateLevelParent()
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

    public void ClearLevel()
    {
        if (_generatedLevelParent == null) CreateLevelParent();

        foreach (Transform child in _generatedLevelParent)
        {
            Destroy(child.gameObject);
        }
        occupiedPositions.Clear();
        _terrainBlocks.Clear();
    }

    public void Generate() {
        ClearLevel();
        GenerateLevel();
        // GenerateClouds();
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

    GameObject CreateTiledObject(GameObject prefab, Vector2 centerPosition, Vector2 size)
    {
        if (prefab == null || size.x <= 0f || size.y <= 0f) return null;

        GameObject instance = Instantiate(prefab, centerPosition, Quaternion.identity, _generatedLevelParent);

        SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = size;
        }
        else {
            instance.transform.localScale = new Vector3(size.x, size.y, instance.transform.localScale.z);
        }

        BoxCollider2D boxCollider = instance.GetComponent<BoxCollider2D>();
        if (boxCollider != null) {
            boxCollider.size = size;
            boxCollider.offset = Vector2.zero;
        }

        return instance;
    }

    private void CreateGroundSegment(int startX, int endXExclusive, int height)
    {
        int width = endXExclusive - startX;
        if (width <= 0 || height <= 0) return;

        float centerX = startX + width * 0.5f - 0.5f;

        CreateTiledObject(
            grassPrefab,
            new Vector2(centerX, height - 1f),
            new Vector2(width, 1f)
        );

        int dirtHeight = height - 1;
        if (dirtHeight > 0) {
            CreateTiledObject(
                dirtPrefab,
                new Vector2(centerX, dirtHeight * 0.5f - 0.5f),
                new Vector2(width, dirtHeight)
            );
        }
    }

    private void CreateDirtColumn(int x, int height)
    {
        int dirtHeight = height - 1;
        if (dirtHeight <= 0) return;

        CreateTiledObject(
            dirtPrefab,
            new Vector2(x, dirtHeight * 0.5f - 0.5f),
            new Vector2(1f, dirtHeight)
        );
    }

    private void FlushGroundSegment(ref int segmentStartX, int currentX, int segmentHeight)
    {
        if (segmentStartX >= 0) {
            CreateGroundSegment(segmentStartX, currentX, segmentHeight);
            segmentStartX = -1;
        }
    }

    private void GenerateLevel()
    {
        Dictionary<NewItem, int> lastItemPositions = new Dictionary<NewItem, int>();
        foreach (var item in newItems) {
            lastItemPositions[item] = -item.minDistance;
        }

        int currentHeight = 0;
        int lastHeight = 0;
        int lastHolePosition = -minDistanceBetweenHoles;
        int groundSegmentStartX = -1;
        int groundSegmentHeight = 0;

        bool riverPlaced = false;
        int lakeLength = Random.Range(minLakeLength, maxLakeLength + 1);
        int lakeStartPosition = Mathf.FloorToInt(levelLength * lakeAttemptPositionFactor);

        for (int i = 0; i < levelLength; i++) {
            if (!riverPlaced && i >= lakeStartPosition && i + lakeLength < levelLength) {
                FlushGroundSegment(ref groundSegmentStartX, i, groundSegmentHeight);

                if (lakeWarningPrefab != null)
                    Instantiate(lakeWarningPrefab, new Vector2(i - 1, (groundThickness + currentHeight) - 0.2f), Quaternion.Euler(0, 0, -15), _generatedLevelParent);

                for (int lakeIdx = 0; lakeIdx < lakeLength; lakeIdx++) {
                    int riverX = i + lakeIdx;
                    Vector2Int lakeBlockPos = new Vector2Int(riverX, 0);
                    if (UpperLakePrefab != null)
                        Instantiate(UpperLakePrefab, new Vector2(riverX, (groundThickness) - 0.12f), Quaternion.identity, _generatedLevelParent);

                    for (int j = groundThickness - 1; j >= 0; j--) {
                        PlaceOrReplaceBlock(new Vector2Int(riverX, j), DownBlockLakePrefab, Quaternion.identity);
                    }
                }
                i += lakeLength - 1;
                riverPlaced = true;
                lastHeight = 0;
                currentHeight = 0;
                continue;
            }

            if (i - lastHolePosition >= minDistanceBetweenHoles && Random.value < holeSpawnChance && i + minHoleWidth < levelLength - maxHoleWidth) {
                FlushGroundSegment(ref groundSegmentStartX, i, groundSegmentHeight);

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

            if (i > 0 && (minDistanceForHeightChangeAttempt <= 1 || i % minDistanceForHeightChangeAttempt == 0)) {
                int heightChange = Random.Range(-maxHeightVariation, maxHeightVariation + 1);
                currentHeight = Mathf.Clamp(currentHeight + heightChange, 0, this.maxHeightVariation);
            }

            int totalGroundHeight = groundThickness + currentHeight;

            if (i > 0 && currentHeight > lastHeight) {
                FlushGroundSegment(ref groundSegmentStartX, i, groundSegmentHeight);
                CreateDirtColumn(i, totalGroundHeight);

                Vector2Int capPos = new Vector2Int(i, groundThickness + currentHeight - 1);
                GameObject capPrefab = grassLeftEdgePrefab != null ? grassLeftEdgePrefab : grassPrefab;
                PlaceOrReplaceBlock(capPos, capPrefab, Quaternion.identity);

                int wallStartY = groundThickness + currentHeight - 2;
                int wallEndY = groundThickness + lastHeight;
                for (int yWall = wallStartY; yWall >= wallEndY; yWall--) {
                    Vector2Int wallPos = new Vector2Int(i, yWall);
                    GameObject wallSegmentPrefab = grassPrefab;
                    PlaceOrReplaceBlock(wallPos, wallSegmentPrefab, Quaternion.Euler(0, 0, 90));
                }

                groundSegmentStartX = i + 1;
                groundSegmentHeight = totalGroundHeight;
            }
            else if (i > 0 && currentHeight < lastHeight) {
                FlushGroundSegment(ref groundSegmentStartX, i - 1, groundSegmentHeight);
                CreateDirtColumn(i - 1, groundThickness + lastHeight);

                Vector2Int capPos = new Vector2Int(i - 1, groundThickness + lastHeight - 1);
                GameObject capPrefab = grassRightEdgePrefab != null ? grassRightEdgePrefab : grassPrefab;
                PlaceOrReplaceBlock(capPos, capPrefab, Quaternion.identity);

                int wallStartY = groundThickness + lastHeight - 2;
                int wallEndY = groundThickness + currentHeight;
                for (int yWall = wallStartY; yWall >= wallEndY; yWall--) {
                    Vector2Int wallPos = new Vector2Int(i - 1, yWall);
                    GameObject wallSegmentPrefab = grassPrefab;
                    PlaceOrReplaceBlock(wallPos, wallSegmentPrefab, Quaternion.Euler(0, 0, -90));
                }

                groundSegmentStartX = i;
                groundSegmentHeight = totalGroundHeight;
            }
            else if (groundSegmentStartX < 0) {
                groundSegmentStartX = i;
                groundSegmentHeight = totalGroundHeight;
            }
            else if (groundSegmentHeight != totalGroundHeight) {
                FlushGroundSegment(ref groundSegmentStartX, i, groundSegmentHeight);
                groundSegmentStartX = i;
                groundSegmentHeight = totalGroundHeight;
            }

            lastHeight = currentHeight;

            foreach (var item in newItems) {
                if (item.prefab != null && i - lastItemPositions[item] >= item.minDistance && Random.value < item.spawnChance) {
                    Vector2 spawnPosition = new Vector2(i, (groundThickness + currentHeight) + item.yOffset);
                    if (!occupiedPositions.Contains(spawnPosition)) {
                        Instantiate(item.prefab, spawnPosition, Quaternion.identity, _generatedLevelParent);
                        lastItemPositions[item] = i;
                        occupiedPositions.Add(spawnPosition);
                    }
                }
            }
        }

        FlushGroundSegment(ref groundSegmentStartX, levelLength, groundSegmentHeight);

        if (flag != null) {
            flag.transform.position = new Vector2(levelLength + 5, groundThickness + lastHeight);
            if (_generatedLevelParent != null) flag.transform.SetParent(_generatedLevelParent);
        }
    }

    // Фишка бесполезная, т.к она ела довольно много ресурсов и дополнительно облка создавались только относительно оси уровня, а не точки максимума уровня (условной возвышенности). Так же оно несло сугубо декоративный характер и выпиисывается из основной темы игры.
    // void GenerateClouds()
    // {
    //     if (cloudPrefabs == null || cloudPrefabs.Length == 0 || cloudHorizontalSpacing <= 0) return;
    //     float currentX = 0f;
    //     while (currentX < levelLength) {
    //         if (Random.value < cloudSpawnChance) {
    //             GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
    //             if (cloudPrefab != null) {
    //                 float cloudY = Random.Range(this.maxHeightVariation + groundThickness + cloudMinY, this.maxHeightVariation + groundThickness + cloudMaxY);
    //                 Instantiate(cloudPrefab, new Vector3(currentX, cloudY, 0), Quaternion.identity, _generatedLevelParent);
    //             }
    //         }
    //         currentX += cloudHorizontalSpacing;
    //     }
    // }

    private void GenerateUnderJava()
    {
        if (levelLength <= 0) return;

        float javaWidth = levelLength;
        float centerX = javaWidth * 0.5f - 0.5f;

        if (mainJavaPrefab != null) {
            CreateTiledObject(
                mainJavaPrefab,
                new Vector2(centerX, javaY - 0.12f),
                new Vector2(javaWidth, 1f)
            );
        }

        if (blockJavaPrefab != null && javaLayerCount > 0)
        {
            float blockCenterY = javaY - (javaLayerCount + 1) * 0.5f;

            CreateTiledObject(
                blockJavaPrefab,
                new Vector2(centerX, blockCenterY),
                new Vector2(javaWidth, javaLayerCount)
            );
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
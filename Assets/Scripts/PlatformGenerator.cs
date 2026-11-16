
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform lastHandPlacedPlatform;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private CrystalPickup crystalPrefab;
    [SerializeField] private PortalExit portalPrefab;
    [SerializeField] private CrystalData blueCrystal;
    [SerializeField] private CrystalData purpleCrystal;

    [Header("Platform Generation")]
    [SerializeField] private int seed = 42;
    [SerializeField] private float minGap = 1.7f;
    [SerializeField] private float maxGap = 2.2f;
    [SerializeField] private float horizontalLimit = 3.5f;
    [SerializeField] private float minHorizontalStep = 1f;
    [SerializeField] private float maxHorizontalStep = 3f;
    [SerializeField] private float generateAhead = 15f;
    [SerializeField] private float cleanupBelow = 20f;
    [SerializeField] private int crystalInterval = 3;

    [Header("Enemy Generation")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyInterval = 5;

    private System.Random random;

    private readonly List<GameObject> platforms =
        new List<GameObject>();

    private readonly List<CrystalPickup> crystals =
        new List<CrystalPickup>();

    private readonly List<GameObject> enemies =
        new List<GameObject>();

    private float nextX;
    private float nextY;
    private int generatedCount;
    private int direction;
    private bool portalSpawned;

    private void Start()
    {
        if (player == null ||
            lastHandPlacedPlatform == null ||
            platformPrefab == null)
        {
            Debug.LogError("PlatformGenerator has missing references.");
            enabled = false;
            return;
        }

        random = new System.Random(seed);

        nextX = lastHandPlacedPlatform.position.x;
        nextY = lastHandPlacedPlatform.position.y;
        direction = nextX >= 0f ? -1 : 1;

        GenerateUntil(player.position.y + generateAhead);
    }

    private void Update()
    {
        if (GameFlow.Instance != null && GameFlow.Instance.IsEnded)
            return;

        GenerateUntil(player.position.y + generateAhead);
        CleanupObjects();
    }

    private float RandomFloat(float min, float max)
    {
        return min + (float)random.NextDouble() * (max - min);
    }

    private void GenerateUntil(float targetY)
    {
        float lowGap = Mathf.Max(0.5f, minGap);
        float highGap = Mathf.Max(lowGap, maxGap);
        float lowStep = Mathf.Max(0.1f, minHorizontalStep);
        float highStep = Mathf.Max(lowStep, maxHorizontalStep);
        float limit = Mathf.Max(1f, horizontalLimit);

        while (nextY < targetY)
        {
            generatedCount++;
            nextY += RandomFloat(lowGap, highGap);

            if (random.NextDouble() < 0.4)
                direction *= -1;

            float step = RandomFloat(lowStep, highStep);
            float candidateX = nextX + direction * step;

            if (candidateX > limit || candidateX < -limit)
            {
                direction *= -1;
                candidateX = nextX + direction * step;
            }

            nextX = Mathf.Clamp(candidateX, -limit, limit);

            Vector3 position = new Vector3(nextX, nextY, 0f);

            GameObject platform = Instantiate(
                platformPrefab,
                position,
                Quaternion.identity,
                transform
            );

            platforms.Add(platform);

            bool puzzleComplete =
                PuzzleController.Instance != null &&
                PuzzleController.Instance.IsComplete;

            bool endless =
                GameFlow.Instance != null &&
                GameFlow.Instance.IsEndlessMode;

            if ((!puzzleComplete || endless) &&
                crystalPrefab != null &&
                generatedCount % Mathf.Max(1, crystalInterval) == 0)
            {
                SpawnCrystal(position);
            }

            if (puzzleComplete &&
                !endless &&
                !portalSpawned &&
                portalPrefab != null)
            {
                SpawnPortal(position);
            }

            if (enemyPrefab != null &&
                generatedCount % Mathf.Max(1, enemyInterval) == 0)
            {
                SpawnEnemy(position);
            }
        }
    }

    private void SpawnCrystal(Vector3 position)
    {
        CrystalPickup pickup = Instantiate(
            crystalPrefab,
            position + Vector3.up * 0.75f,
            Quaternion.identity,
            transform
        );

        CrystalData data = generatedCount % 2 == 0
            ? purpleCrystal
            : blueCrystal;

        pickup.Configure(generatedCount, data);
        crystals.Add(pickup);
    }

    private void SpawnPortal(Vector3 position)
    {
        Instantiate(
            portalPrefab,
            position + Vector3.up * 1.2f,
            Quaternion.identity,
            transform
        );

        portalSpawned = true;
    }

    private void SpawnEnemy(Vector3 position)
    {
        Vector3 enemyPosition = position + Vector3.up * 1.5f;

        enemyPosition.x = Mathf.Clamp(
            enemyPosition.x,
            -1.5f,
            1.5f
        );

        GameObject enemy = Instantiate(
            enemyPrefab,
            enemyPosition,
            Quaternion.identity,
            transform
        );

        enemies.Add(enemy);
    }

    private void CleanupObjects()
    {
        float cutoff = player.position.y - cleanupBelow;

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i] == null)
            {
                platforms.RemoveAt(i);
                continue;
            }

            if (platforms[i].transform.position.y < cutoff)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }

        for (int i = crystals.Count - 1; i >= 0; i--)
        {
            if (crystals[i] == null)
            {
                crystals.RemoveAt(i);
                continue;
            }

            if (crystals[i].transform.position.y < cutoff)
            {
                Destroy(crystals[i].gameObject);
                crystals.RemoveAt(i);
            }
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            if (enemies[i].transform.position.y < cutoff)
            {
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
            }
        }
    }
}

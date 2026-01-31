using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesRemainingText;

    [Header("Enemy Prefabs")]
    public GameObject meleePrefab;
    public GameObject rangedPrefab;
    public GameObject tankPrefab;
    public GameObject fastPrefab;
    public GameObject bossPrefab;

    [Header("Wave Settings")]
    public int baseEnemyCount = 3;
    public float timeBetweenWaves = 5f;
    public int wavesBetweenBosses = 5;
    public float spawnRadius = 7f; // distance from player to spawn enemies

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    private List<GameObject> unlockedEnemyTypes = new List<GameObject>();
    private int nextBossWave;

    void Start()
    {
        unlockedEnemyTypes.Add(meleePrefab); // start only with melee enemies
        nextBossWave = wavesBetweenBosses;

        UpdateEnemiesRemainingText();
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (enemiesAlive <= 0 && !isSpawning)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        currentWave++;
        waveText.text = "Wave " + currentWave;

        yield return new WaitForSeconds(timeBetweenWaves);

        if (currentWave == nextBossWave)
        {
            // Spawn boss wave
            SpawnBoss();
        }
        else
        {
            // Spawn normal enemies
            int enemyCount = baseEnemyCount + currentWave * 2;
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject prefab = unlockedEnemyTypes[Random.Range(0, unlockedEnemyTypes.Count)];
                SpawnEnemyNearPlayer(prefab);
                yield return new WaitForSeconds(0.3f);
            }

            // Optional ambush wave every 3 waves
            if (currentWave % 3 == 0)
            {
                SpawnAmbush();
            }
        }

        isSpawning = false;
    }

    void SpawnEnemyNearPlayer(GameObject prefab)
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
        GameObject e = Instantiate(prefab, spawnPos, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();

        // Scale difficulty
        enemy.speed += currentWave * 0.2f;
        enemy.health += currentWave;

        enemiesAlive++;
        UpdateEnemiesRemainingText();

        enemy.OnDeath += () =>
        {
            enemiesAlive--;
            UpdateEnemiesRemainingText();
        };
    }

    void SpawnBoss()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
        GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        Enemy bossEnemy = boss.GetComponent<Enemy>();

        enemiesAlive++;
        UpdateEnemiesRemainingText();

        bossEnemy.OnDeath += () =>
        {
            enemiesAlive--;
            UpdateEnemiesRemainingText();
            UnlockNextEnemyType();
        };
    }

    void UnlockNextEnemyType()
    {
        // Unlock new enemy type sequentially
        if (!unlockedEnemyTypes.Contains(rangedPrefab))
            unlockedEnemyTypes.Add(rangedPrefab);
        else if (!unlockedEnemyTypes.Contains(tankPrefab))
            unlockedEnemyTypes.Add(tankPrefab);
        else if (!unlockedEnemyTypes.Contains(fastPrefab))
            unlockedEnemyTypes.Add(fastPrefab);

        // Schedule next boss wave
        nextBossWave += wavesBetweenBosses;
    }

    void SpawnAmbush()
    {
        int ambushCount = 3; // small ambush
        for (int i = 0; i < ambushCount; i++)
        {
            GameObject prefab = unlockedEnemyTypes[Random.Range(0, unlockedEnemyTypes.Count)];
            SpawnEnemyNearPlayer(prefab);
        }
    }

    void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null)
            enemiesRemainingText.text = "Enemies Remaining: " + enemiesAlive;
    }
}

using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public ShopUI shopUI;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI coinText;

    [Header("Enemy Prefabs")]
    public GameObject meleePrefab;
    public GameObject rangedPrefab;
    public GameObject tankPrefab;
    public GameObject fastPrefab;
    public GameObject bossPrefab;

    [Header("Wave Settings")]
    public int baseEnemyCount = 3;
    public float timeBetweenWaves = 1f;
    public int wavesBetweenBosses = 5;
    public float spawnRadius = 7f;

    [Header("Currency")]
    public int coins = 0;

    [Header("Boss Arena")]
    public BossArena bossArena;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    private List<GameObject> unlockedEnemyTypes = new List<GameObject>();
    private int nextBossWave;

    void Start()
    {
        if (player == null) Debug.LogError("Player not assigned!");
        if (shopUI == null) Debug.LogError("ShopUI not assigned!");

        unlockedEnemyTypes.Add(meleePrefab);
        nextBossWave = wavesBetweenBosses;

        UpdateEnemiesRemainingText();
        UpdateCoinUI();

        Debug.Log("Starting first wave...");
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        UpdateCoinUI();
    }

    // =========================
    // Wave Management
    // =========================
    public IEnumerator StartNextWave()
    {
        isSpawning = true;
        currentWave++;
        waveText.text = "Wave " + currentWave;
        Debug.Log($"Wave {currentWave} started!");

        yield return new WaitForSeconds(timeBetweenWaves);

        if (currentWave == nextBossWave)
        {
            Debug.Log("Spawning boss wave!");
            SpawnBoss();
        }
        else
        {
            int enemyCount = baseEnemyCount + currentWave * 2;
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject prefab = unlockedEnemyTypes[Random.Range(0, unlockedEnemyTypes.Count)];
                SpawnEnemyNearPlayer(prefab);
                yield return new WaitForSeconds(0.3f);
            }

            if (currentWave % 3 == 0)
            {
                Debug.Log("Spawning ambush wave!");
                SpawnAmbush();
            }
        }

        isSpawning = false;
    }

    // =========================
    // Enemy Spawning
    // =========================
    void SpawnEnemyNearPlayer(GameObject prefab)
    {
        if (prefab == null) return;

        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
        GameObject e = Instantiate(prefab, spawnPos, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        if (enemy == null) { Debug.LogWarning("Enemy prefab missing Enemy script!"); return; }

        enemy.speed += currentWave * 0.2f;
        enemy.health += currentWave;

        enemiesAlive++;
        UpdateEnemiesRemainingText();

        enemy.OnDeath += () =>
        {
            enemiesAlive--;
            UpdateEnemiesRemainingText();
            Debug.Log("Enemy died! Remaining: " + enemiesAlive);

            if (enemiesAlive <= 0 && !isSpawning)
            {
                Debug.Log("Wave ended! Showing shop...");
                if (shopUI != null)
                    shopUI.Show();
            }
        };

        Debug.Log("Spawned enemy: " + prefab.name);
    }

    void SpawnBoss()
        {
            if (bossPrefab == null) return;

            Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            Enemy bossEnemy = boss.GetComponent<Enemy>();
            if (bossEnemy == null)
            {
                Debug.LogWarning("Boss prefab missing Enemy script!");
                return;
            }

            enemiesAlive++;
            UpdateEnemiesRemainingText();

            // 🔒 Activate arena
            if (bossArena != null)
                bossArena.ActivateArena(bossEnemy);

            bossEnemy.OnDeath += () =>
            {
                enemiesAlive--;
                UpdateEnemiesRemainingText();
                Debug.Log("Boss defeated!");

                UnlockNextEnemyType();

                if (enemiesAlive <= 0 && !isSpawning)
                {
                    shopUI.Show();
                }
            };
        }


    void SpawnAmbush()
    {
        int ambushCount = 3;
        for (int i = 0; i < ambushCount; i++)
        {
            GameObject prefab = unlockedEnemyTypes[Random.Range(0, unlockedEnemyTypes.Count)];
            SpawnEnemyNearPlayer(prefab);
        }
    }

    void UnlockNextEnemyType()
    {
        if (!unlockedEnemyTypes.Contains(rangedPrefab)) unlockedEnemyTypes.Add(rangedPrefab);
        else if (!unlockedEnemyTypes.Contains(tankPrefab)) unlockedEnemyTypes.Add(tankPrefab);
        else if (!unlockedEnemyTypes.Contains(fastPrefab)) unlockedEnemyTypes.Add(fastPrefab);

        nextBossWave += wavesBetweenBosses;
        Debug.Log("Next enemy type unlocked! Next boss wave: " + nextBossWave);
    }

    // =========================
    // UI & Coins
    // =========================
    void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null)
            enemiesRemainingText.text = "Enemies Remaining: " + enemiesAlive;
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinUI();
        Debug.Log($"Added {amount} coins. Total: {coins}");
    }

    public bool TrySpendCoins(int amount)
    {
        if (coins < amount) return false;
        coins -= amount;
        UpdateCoinUI();
        return true;
    }

    // Called by shop button
    public void StartNextWaveFromShop()
    {
        if (!isSpawning)
        {
            Debug.Log("Starting next wave from shop...");
            StartCoroutine(StartNextWave());
        }
    }
}

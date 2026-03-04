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

    [Header("GameOver")]
    public DeathPanelManager deathPanelManager;

    [Header("Scaling Constants")]
    public float healthMultiplier = 1.15f; // 15% increase per wave
    public float damageMultiplier = 1.10f; // 10% increase per wave
    public float speedIncrement = 0.05f;   // Tiny speed boosts
    public float maxEnemySpeed = 4.5f;     // Hard cap so player can still kite

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
    
    // Calculate how many enemies to spawn this wave
    // Starts at base (3) + 2 per wave (Wave 1 = 5, Wave 2 = 7, etc.)
    int enemyCount = baseEnemyCount + (currentWave * 2);

    yield return new WaitForSeconds(timeBetweenWaves);

    // --- BOSS WAVE CHECK ---
    if (currentWave == nextBossWave)
    {
        Debug.Log("BOSS ENCOUNTER STARTED");
        SpawnBoss();
    }
    else
    {
        // --- REGULAR WAVE SPAWNING ---
        for (int i = 0; i < enemyCount; i++)
        {
            // Pick a random prefab from the currently unlocked types
            GameObject prefab = unlockedEnemyTypes[UnityEngine.Random.Range(0, unlockedEnemyTypes.Count)];
            
            SpawnEnemyWithScaling(prefab);

            // Slight delay between individual spawns so they don't all overlap
            yield return new WaitForSeconds(0.3f);
        }

        // --- AMBUSH LOGIC ---
        // Every 3 waves (that aren't boss waves), spawn an extra burst of enemies
        if (currentWave % 3 == 0)
        {
            Debug.Log("AMBUSH!");
            SpawnAmbush();
        }
    }

    isSpawning = false;
}

// I've moved the logic into this helper function to keep StartNextWave clean
    void SpawnEnemyWithScaling(GameObject prefab)
    {
        if (prefab == null) return;

        // 1. Instantiate the enemy
        Vector2 spawnPos = (Vector2)player.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
        GameObject e = Instantiate(prefab, spawnPos, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();

        if (enemy != null)
        {
            // 2. APPLY BETTER SCALING MATH
            
            // Exponential Health: Grows by 15% every wave (1.15 multiplier)
            float scaledHealth = enemy.health * Mathf.Pow(healthMultiplier, currentWave - 1);
            enemy.health = Mathf.RoundToInt(scaledHealth);
            enemy.maxHealth = enemy.health; 

            // Exponential Damage: Grows by 10% every wave
            float scaledDamage = enemy.damage * Mathf.Pow(damageMultiplier, currentWave - 1);
            enemy.damage = Mathf.RoundToInt(scaledDamage);

            // Capped Speed: Increments slowly but stops at maxEnemySpeed (e.g., 4.5)
            float newSpeed = enemy.speed + (currentWave * speedIncrement);
            enemy.speed = Mathf.Min(newSpeed, maxEnemySpeed);

            // 3. Update Wave Tracking
            enemiesAlive++;
            UpdateEnemiesRemainingText();

            // 4. Handle Death
            enemy.OnDeath += () => 
            {
                enemiesAlive--;
                UpdateEnemiesRemainingText();

                // If wave is cleared, show shop
                if (enemiesAlive <= 0 && !isSpawning)
                {
                    if (shopUI != null) shopUI.Show();
                }
            };
        }
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

    if (enemy != null)
    {
        // --- IMPROVED SCALING MATH ---
        
        // 1. Exponential Health: Health = Base * (1.15 ^ Wave)
        float scaledHealth = enemy.health * Mathf.Pow(healthMultiplier, currentWave - 1);
        enemy.health = Mathf.RoundToInt(scaledHealth);
        enemy.maxHealth = enemy.health; // Ensure UI shows full bar

        // 2. Exponential Damage: Damage = Base * (1.10 ^ Wave)
        float scaledDamage = enemy.damage * Mathf.Pow(damageMultiplier, currentWave - 1);
        enemy.damage = Mathf.RoundToInt(scaledDamage);

        // 3. Capped Speed: Speed increases but never passes maxEnemySpeed
        float newSpeed = enemy.speed + (currentWave * speedIncrement);
        enemy.speed = Mathf.Min(newSpeed, maxEnemySpeed);

        // Register Death Event
        enemy.OnDeath += () => HandleEnemyDeath();

        int extraCoins = Mathf.FloorToInt(currentWave / 5f); 
        enemy.coinsToDrop += extraCoins;
        
        enemiesAlive++;
        UpdateEnemiesRemainingText();
    }
}

void SpawnBoss()
{
    if (bossPrefab == null) return;

    Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
    GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    Enemy boss = bossObj.GetComponent<Enemy>();

    if (boss != null)
    {
        // Bosses get a much higher health pool (e.g., 5x a normal enemy of that wave)
        float scaledHealth = (boss.health * 5f) * Mathf.Pow(healthMultiplier, currentWave - 1);
        boss.health = Mathf.RoundToInt(scaledHealth);
        boss.maxHealth = boss.health;

        // Bosses get a damage boost too
        boss.damage = Mathf.RoundToInt(boss.damage * 1.5f);

        enemiesAlive++;
        UpdateEnemiesRemainingText();
        
        // Arena Setup
        if (bossArena != null)
        {
            BossArena arenaClone = Instantiate(bossArena, Vector3.zero, Quaternion.identity);
            arenaClone.ActivateArena(boss);
        }

        boss.OnDeath += () => {
            HandleEnemyDeath();
            UnlockNextEnemyType();
            SoundManager.instance.PlayMusic(SoundManager.instance.gameMusic);
        };
    }
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
        bool allUnlocked = false;

        // Sequential unlocking
        if (!unlockedEnemyTypes.Contains(rangedPrefab)) 
            unlockedEnemyTypes.Add(rangedPrefab);
        else if (!unlockedEnemyTypes.Contains(tankPrefab)) 
            unlockedEnemyTypes.Add(tankPrefab);
        else if (!unlockedEnemyTypes.Contains(fastPrefab)) 
            unlockedEnemyTypes.Add(fastPrefab);
        else
            allUnlocked = true;

        if (allUnlocked)
        {
            Debug.Log("MAX HEAT: All enemy types are now spawning!");
            // Optional: Increase spawn rate or base enemy count for "Endless" feel
            baseEnemyCount += 5; 
        }

        nextBossWave += wavesBetweenBosses;
    }

    void HandleEnemyDeath()
    {
    enemiesAlive--;
    UpdateEnemiesRemainingText();
    if (enemiesAlive <= 0 && !isSpawning)
    {
        if (shopUI != null) shopUI.Show();
    }
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

    public void EndGame()
    {
        // Simple score formula: (Waves * 100) + Coins
        int finalScore = (currentWave * 100) + coins;
        
        if (deathPanelManager != null)
        {
            deathPanelManager.ToggleDeathPanel(finalScore);
        }
        else
        {
            Debug.LogError("DeathPanelManager not assigned to GameManager!");
        }
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}

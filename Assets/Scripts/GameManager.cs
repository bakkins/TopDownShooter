using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Spawning")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;    // NEW: array for all enemy types
    public GameObject bossPrefab;        // separate prefab for boss

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesRemainingText;

    [Header("Settings")]
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
        UpdateEnemiesRemainingText();
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (enemiesAlive <= 0 && !isSpawning)
            StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        currentWave++;
        waveText.text = "Wave " + currentWave;

        yield return new WaitForSeconds(timeBetweenWaves);

        // Boss every 10 waves
        if (currentWave % 10 == 0 && bossPrefab != null)
        {
            SpawnBoss();
        }
        else
        {
            int enemyCount = 3 + currentWave * 2;

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Pick a random prefab from the array
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefab = enemyPrefabs[index];

        GameObject e = Instantiate(prefab, spawn.position, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();

        // Scale stats per wave
        enemy.health += currentWave;
        enemy.speed += currentWave * 0.2f;

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
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject boss = Instantiate(bossPrefab, spawn.position, Quaternion.identity);
        Enemy bossEnemy = boss.GetComponent<Enemy>();

        bossEnemy.health *= 5;   // Boss is tougher
        bossEnemy.speed = 1f;

        enemiesAlive++;
        bossEnemy.OnDeath += () =>
        {
            enemiesAlive--;
            UpdateEnemiesRemainingText();
        };
    }

    void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null)
            enemiesRemainingText.text = "Enemies Remaining: " + enemiesAlive;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public Text waveText;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    public float timeBetweenWaves = 5f;

    void Start()
    {
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

        int enemyCount = 3 + currentWave * 2;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject e = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();

        enemy.speed += currentWave * 0.2f;
        enemy.health += currentWave;

        enemiesAlive++;
        enemy.OnDeath += () => enemiesAlive--;
    }
}

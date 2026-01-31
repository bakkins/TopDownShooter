using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public GameObject[] pickupPrefabs;
    public float spawnInterval = 10f;
    public float spawnRadius = 8f;
    public Transform player;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPickup();
            timer = 0f;
        }
    }

    void SpawnPickup()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
        int index = Random.Range(0, pickupPrefabs.Length);
        Instantiate(pickupPrefabs[index], spawnPos, Quaternion.identity);
    }
}

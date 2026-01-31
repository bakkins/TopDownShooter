using UnityEngine;

public class AutomaticAbility : MonoBehaviour
{
    public GameObject abilityPrefab;   // E.g., a grenade prefab
    public float cooldown = 5f;        // Seconds between abilities
    public float range = 5f;           // Distance from player to spawn

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            TriggerAbility();
            timer = 0f;
        }
    }

    void TriggerAbility()
    {
        // Spawn around player randomly within range
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * range;
        Instantiate(abilityPrefab, spawnPos, Quaternion.identity);
    }
}

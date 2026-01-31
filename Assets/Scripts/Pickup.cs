using UnityEngine;
using System.Collections;

public enum PickupType { Health, Shield, Speed, Damage }

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public PickupType type;
    public int amount = 20;           // Health/shield amount or percentage for buffs
    public float duration = 5f;       // Duration for temporary buffs (speed/damage)

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerStats stats = collision.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (type)
        {
            case PickupType.Health:
                stats.Heal(amount);
                break;

            case PickupType.Shield:
                stats.ApplyShield(amount);
                break;

            case PickupType.Speed:
                stats.ApplySpeedBoost(1 + (amount / 100f)); // e.g., amount = 50 → 1.5x speed
                stats.StartCoroutine(ResetSpeedAfter(stats, duration));
                break;

            case PickupType.Damage:
                stats.ApplyDamageBoost(1 + (amount / 100f)); // e.g., amount = 100 → 2x damage
                stats.StartCoroutine(ResetDamageAfter(stats, duration));
                break;
        }

        Destroy(gameObject);
    }

    // Reset speed multiplier after duration
    private IEnumerator ResetSpeedAfter(PlayerStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);
        stats.ApplySpeedBoost(1f);
    }

    // Reset damage multiplier after duration
    private IEnumerator ResetDamageAfter(PlayerStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);
        stats.ApplyDamageBoost(1f);
    }
}

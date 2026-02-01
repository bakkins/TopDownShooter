using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int amount = 10;
    public float duration = 5f;

    private static GameManager gm;

    void Awake()
    {
        if (gm == null)
            gm = Object.FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (type)
        {
            case PickupType.Health:
                stats.AddHealth(amount);
                break;

            case PickupType.Shield:
                stats.AddShield(amount);
                break;

            case PickupType.SpeedBoost:
                stats.ApplySpeedBoost(duration);
                break;

            case PickupType.DamageBoost:
                stats.ApplyDamageBoost(duration);
                break;

            case PickupType.Coins:
                if (gm != null)
                    gm.AddCoins(amount);
                break;
        }

        Destroy(gameObject);
    }
}

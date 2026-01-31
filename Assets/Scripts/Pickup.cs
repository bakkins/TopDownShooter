using UnityEngine;

public enum PickupType { Health, Shield, Speed, Damage }

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int amount = 20;
    public float duration = 5f; // for temporary boosts

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                switch (type)
                {
                    case PickupType.Health:
                        stats.Heal(amount);
                        break;
                    case PickupType.Shield:
                        stats.ApplyShield(duration);
                        break;
                    case PickupType.Speed:
                        stats.ApplySpeedBoost(amount, duration);
                        break;
                    case PickupType.Damage:
                        stats.ApplyDamageBoost(amount, duration);
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}

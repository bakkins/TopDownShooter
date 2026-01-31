using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    [Header("Shield")]
    public int maxShield = 50;
    private int currentShield;
    public Slider shieldBar;

    [Header("Buffs")]
    public float speedMultiplier = 1f;       // used by PlayerController
    public float damageMultiplier = 1f;      // used by weapons

    [Header("Death Settings")]
    public bool disableControllerOnDeath = true;

    private PlayerController playerController;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        currentShield = maxShield;
        if (shieldBar != null)
        {
            shieldBar.maxValue = maxShield;
            shieldBar.value = currentShield;
        }

        playerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Apply damage to player: shield absorbs first, then health
    /// </summary>
    public void TakeDamage(int amount)
    {
        // Damage to shield first
        if (currentShield > 0)
        {
            int shieldDamage = Mathf.Min(amount, currentShield);
            currentShield -= shieldDamage;
            amount -= shieldDamage;

            if (shieldBar != null)
                shieldBar.value = currentShield;
        }

        // Remaining damage to health
        if (amount > 0)
        {
            currentHealth -= amount;
            if (healthBar != null)
                healthBar.value = currentHealth;

            if (currentHealth <= 0)
                Die();
        }
    }

    /// <summary>
    /// Heal health via pickups
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    /// <summary>
    /// Recharge shield via pickups
    /// </summary>
    public void RechargeShield(int amount)
    {
        currentShield += amount;
        currentShield = Mathf.Min(currentShield, maxShield);
        if (shieldBar != null)
            shieldBar.value = currentShield;
    }

    /// <summary>
    /// Pickups will call these methods
    /// </summary>
    public void ApplyShield(int amount)
    {
        RechargeShield(amount);
    }

    public void ApplySpeedBoost(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void ApplyDamageBoost(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    void Die()
    {
        if (disableControllerOnDeath && playerController != null)
            playerController.enabled = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Optional getters
    public int GetHealth() => currentHealth;
    public int GetShield() => currentShield;
    public bool IsAlive() => currentHealth > 0;
}

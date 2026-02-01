using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Shield")]
    public int maxShield = 50;
    public int currentShield;

    [Header("Movement")]
    public float baseMoveSpeed = 5f;
    [HideInInspector] public float moveSpeedMultiplier = 1f;

    [Header("Damage")]
    [HideInInspector] public float damageMultiplier = 1f;

    [Header("UI")]
    public Slider healthBar;
    public Slider shieldBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        UpdateUI();
    }

    // =========================
    // DAMAGE HANDLING
    // =========================
    public void TakeDamage(int damage)
    {
        if (currentShield > 0)
        {
            int shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0)
            currentHealth -= damage;

        UpdateUI();

        if (currentHealth <= 0)
            Die();
    }

    // =========================
    // PICKUP METHODS
    // =========================
    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();
    }

    public void AddShield(int amount)
    {
        currentShield = Mathf.Min(currentShield + amount, maxShield);
        UpdateUI();
    }

    public void ApplySpeedBoost(float duration)
    {
        StopCoroutine(nameof(SpeedBoostCoroutine));
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    public void ApplyDamageBoost(float duration)
    {
        StopCoroutine(nameof(DamageBoostCoroutine));
        StartCoroutine(DamageBoostCoroutine(duration));
    }

    // =========================
    // COROUTINES
    // =========================
    IEnumerator SpeedBoostCoroutine(float duration)
    {
        moveSpeedMultiplier = 1.5f;
        yield return new WaitForSeconds(duration);
        moveSpeedMultiplier = 1f;
    }

    IEnumerator DamageBoostCoroutine(float duration)
    {
        damageMultiplier = 2f;
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
    }

    // =========================
    // UI
    // =========================
    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (shieldBar != null)
        {
            shieldBar.maxValue = maxShield;
            shieldBar.value = currentShield;
        }
    }

    // =========================
    // DEATH
    // =========================
    void Die()
    {
        GetComponent<PlayerController>().enabled = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

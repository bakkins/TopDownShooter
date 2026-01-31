using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Core Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Boost Stats")]
    public float speedMultiplier = 1f;      // multiplier applied to movement speed
    public float damageMultiplier = 1f;     // multiplier applied to bullets
    private bool shieldActive = false;

    [Header("References")]
    public PlayerController controller;     // your movement/shooting script
    public PlayerWeaponController weaponController; // weapon script
    public PlayerHealth healthUI;           // health slider UI

    void Start()
    {
        currentHealth = maxHealth;
        if (healthUI != null)
        {
            healthUI.SetMaxHealth(maxHealth);
            healthUI.UpdateHealth(currentHealth);
        }
    }

    // ---------------- Health ----------------
    public void TakeDamage(int amount)
    {
        if (shieldActive)
            return; // shield blocks all damage

        currentHealth -= amount;
        if (healthUI != null)
            healthUI.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.UpdateHealth(currentHealth);
    }

    void Die()
    {
        controller.enabled = false;
        weaponController.enabled = false;
        Debug.Log("Player Died!");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // ---------------- Shield ----------------
    public void ApplyShield(float duration)
    {
        if (!shieldActive)
            StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        shieldActive = true;
        // Optional: add visual effect
        yield return new WaitForSeconds(duration);
        shieldActive = false;
    }

    // ---------------- Speed Boost ----------------
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        controller.moveSpeed *= speedMultiplier; // multiply existing speed
        yield return new WaitForSeconds(duration);
        controller.moveSpeed /= speedMultiplier;
        speedMultiplier = 1f;
    }

    // ---------------- Damage Boost ----------------
    public void ApplyDamageBoost(float multiplier, float duration)
    {
        StartCoroutine(DamageBoostRoutine(multiplier, duration));
    }

    private IEnumerator DamageBoostRoutine(float multiplier, float duration)
    {
        damageMultiplier = multiplier;
        weaponController.currentWeapon.damage = Mathf.RoundToInt(weaponController.currentWeapon.damage * damageMultiplier);
        yield return new WaitForSeconds(duration);
        weaponController.currentWeapon.damage = Mathf.RoundToInt(weaponController.currentWeapon.damage / damageMultiplier);
        damageMultiplier = 1f;
    }
}

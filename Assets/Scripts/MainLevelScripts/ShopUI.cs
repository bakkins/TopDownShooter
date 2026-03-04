using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;
    public GameObject player; // assign player here

    [Header("UI Elements")]
    public GameObject shopPanel;
    public Button healthButton;
    public Button shieldButton;
    public Button damageButton;
    public Button fireRateButton;

    [Header("Costs")]
    public int healthCost = 10;
    public int shieldCost = 10;
    public int damageCost = 15;
    public int fireRateCost = 20;

    private PlayerStats playerStats;
    private PlayerWeaponController playerWeapon;

    private bool visible = false;

    void Start()
    {
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerWeapon = player.GetComponent<PlayerWeaponController>();
        }

        Hide(); // hide on start
    }

    void Update()
    {
        if (visible)
            UpdateButtons();
    }

    // =========================
    // SHOW/HIDE
    // =========================
    public void Show()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            visible = true;
            Debug.Log("Shop opened! ActiveInHierarchy = " + shopPanel.activeInHierarchy);
        }
    }

    public void Hide()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            visible = false;
            Debug.Log("Shop closed!");
        }
    }

    public void StartNextWave()
    {
        Hide();
        if (gameManager != null)
            gameManager.StartNextWaveFromShop();
    }

    // =========================
    // BUTTON UPDATES
    // =========================
    void UpdateButtons()
    {
        if (gameManager == null) return;

        int coins = gameManager.coins;

        if (healthButton != null) healthButton.interactable = coins >= healthCost;
        if (shieldButton != null) shieldButton.interactable = coins >= shieldCost;
        if (damageButton != null) damageButton.interactable = coins >= damageCost;
        if (fireRateButton != null) fireRateButton.interactable = coins >= fireRateCost;
    }

    // =========================
    // BUTTON CALLBACKS
    // =========================
    public void BuyHealth()
    {
        if (gameManager != null && playerStats != null && gameManager.TrySpendCoins(healthCost))
        {
            playerStats.maxHealth += 20;
            playerStats.AddHealth(20);
            Debug.Log("Bought Health Upgrade!");
        }
    }

    public void BuyShield()
    {
        if (gameManager != null && playerStats != null && gameManager.TrySpendCoins(shieldCost))
        {
            playerStats.maxShield += 10;
            playerStats.AddShield(10);
            Debug.Log("Bought Shield Upgrade!");
        }
    }

    public void BuyDamage()
    {
        if (gameManager != null && playerStats != null && gameManager.TrySpendCoins(damageCost))
        {
            playerStats.damageMultiplier += 0.1f;
            Debug.Log("Bought Damage Upgrade! New multiplier: " + playerStats.damageMultiplier);
        }
    }

    public void BuyFireRate()
    {
        if (gameManager != null && playerWeapon != null && gameManager.TrySpendCoins(fireRateCost))
        {
            playerWeapon.fireRate = Mathf.Max(0.05f, playerWeapon.fireRate - 0.02f);
            Debug.Log("Bought Fire Rate Upgrade! New fire rate: " + playerWeapon.fireRate);
        }
    }
}

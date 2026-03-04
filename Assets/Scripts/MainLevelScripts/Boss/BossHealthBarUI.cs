using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Image fillImage; 
    private Enemy targetBoss;

    public void Initialize(Enemy boss)
    {
        targetBoss = boss;
        
        // Fixes the "Shorter/Squashed" bug
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null) rect.localScale = Vector3.one;

        UpdateUI();
    }

    void Update()
    {
        // FIX: If the boss is destroyed, delete this health bar immediately
        if (targetBoss == null)
        {
            Destroy(gameObject);
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (targetBoss == null || fillImage == null) return;

        // FIX: If maxHealth is 0, the bar stays black. 
        // We use (float) to get a decimal like 0.75f
        if (targetBoss.maxHealth > 0)
        {
            fillImage.fillAmount = (float)targetBoss.health / targetBoss.maxHealth;
        }
    }
}
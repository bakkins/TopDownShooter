using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int damage = 1;              // ⚡ Added this
    public bool piercing = false;

    [Header("Fire Settings")]
    public float fireRate = 0.2f;
    public int bulletCount = 1;         // for spread
    public float spreadAngle = 15f;     // angle between bullets
}

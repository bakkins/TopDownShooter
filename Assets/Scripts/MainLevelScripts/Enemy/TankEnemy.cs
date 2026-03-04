using UnityEngine;
using System;

public class TankEnemy : Enemy
{
    public int armorValue = 1; // Subtracts 1 from every hit

    protected override void Start()
    {
        base.Start();
        knockbackForce = 0.2f; // Barely moves when hit
    }

    public override void TakeDamage(int damageAmount)
    {
        // Subtract armor, but ensure at least 1 damage is dealt
        int actualDamage = Mathf.Max(1, damageAmount - armorValue);
        base.TakeDamage(actualDamage);
    }
}
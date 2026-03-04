using UnityEngine;
using System;

public class FastEnemy : Enemy
{
    public float boostMultiplier = 2f;
    public float boostDuration = 1.5f;
    private bool isBoosting = false;

    protected override void Update()
    {
        if (player == null) return;

        // Logic: If close to player, "Dash"
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < 4f && !isBoosting)
        {
            StartCoroutine(SpeedBoost());
        }

        base.Update();
    }

    System.Collections.IEnumerator SpeedBoost()
    {
        isBoosting = true;
        float originalSpeed = speed;
        speed *= boostMultiplier;
        yield return new WaitForSeconds(boostDuration);
        speed = originalSpeed;
        yield return new WaitForSeconds(3f); // Cooldown
        isBoosting = false;
    }
}
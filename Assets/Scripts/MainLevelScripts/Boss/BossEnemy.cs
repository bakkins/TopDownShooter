using UnityEngine;
using System;

public class BossEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        
        // 1. Switch Music
        SoundManager.instance?.PlayMusic(SoundManager.instance.bossMusic);

        // 2. Trigger Arena
        BossArena arena = UnityEngine.Object.FindFirstObjectByType<BossArena>();
        if (arena != null) arena.ActivateArena(this);
    }

    protected override void Die()
    {
        // Switch Music back
        SoundManager.instance?.PlayMusic(SoundManager.instance.gameMusic);
        base.Die();
    }
}
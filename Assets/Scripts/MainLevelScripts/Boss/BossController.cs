using UnityEngine;
using System.Collections;

public enum BossState { Chasing, Dashing, Shooting, Idle }

public class BossController : MonoBehaviour
{
    [Header("General Settings")]
    public BossState currentState = BossState.Idle;
    private Enemy enemyBase;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Dash Move")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 3f;
    private bool canDash = true;

    [Header("Shoot Move")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    public int projectilesPerBurst = 3;
    private bool canShoot = true;

    void Start()
    {
        enemyBase = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // IMPORTANT: Set state to Chasing so the BossLogicLoop starts picking attacks
        currentState = BossState.Chasing; 

        StartCoroutine(BossLogicLoop());
    }

    IEnumerator BossLogicLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // Randomly pick a move if we aren't currently doing one
            if (currentState == BossState.Chasing)
            {
                float chance = Random.value;
                if (chance < 0.4f && canDash) 
                    StartCoroutine(DashAttack());
                else if (chance < 0.8f && canShoot) 
                    StartCoroutine(ShootAttack());
            }
        }
    }

    void Update()
    {
        if (player == null || currentState == BossState.Idle) return;

        // Use base Enemy speed for chasing, but stop moving during special attacks
        if (currentState == BossState.Chasing)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)dir * enemyBase.speed * Time.deltaTime;
        }
    }

    // =========================
    // MOVE 1: DASH ATTACK
    // =========================
    IEnumerator DashAttack()
    {
        currentState = BossState.Dashing;
        canDash = false;

        // Telegraph: Flash red or stay still for a moment
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red; 
        yield return new WaitForSeconds(0.5f);
        sr.color = originalColor;

        // Perform Dash
        Vector2 dashDir = (player.position - transform.position).normalized;
        rb.linearVelocity = dashDir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        currentState = BossState.Chasing;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // =========================
    // MOVE 2: SHOOT ATTACK
    // =========================
    IEnumerator ShootAttack()
    {
        currentState = BossState.Shooting;
        canShoot = false;

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            if (player == null) break;
            
            ShootProjectile();
            yield return new WaitForSeconds(0.2f); // Delay between burst shots
        }

        currentState = BossState.Chasing;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = (player.position - transform.position).normalized;
        
        // Ensure the projectile has a simple script to move it forward
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            projRb.linearVelocity = dir * 8f;
        }
        SoundManager.instance.PlaySFX(SoundManager.instance.bossShoot);
    }
}
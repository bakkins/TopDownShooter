using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D rb;
    private Vector2 movement;

    [Header("Mouse / Aim")]
    public Camera cam;
    private Vector2 mousePos;

    // Reference to PlayerStats
    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
            Debug.LogError("PlayerStats component not found on Player!");
    }

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Mouse position
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (stats == null) return;

        // Move player with speed multiplier from pickups
        float effectiveSpeed = stats.baseMoveSpeed * stats.moveSpeedMultiplier;
        rb.MovePosition(rb.position + movement * effectiveSpeed * Time.fixedDeltaTime);

        // Rotate player to face mouse
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}

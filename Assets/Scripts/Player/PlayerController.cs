using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public UnityEngine.Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0f;

    void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            fireTimer += Time.deltaTime;
            if (Input.GetMouseButton(0) && fireTimer >= fireRate)
                {
                    Shoot();
                    fireTimer = 0f;
                }
        }

    void Shoot()
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

    void FixedUpdate()
        {
            // Move player
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            // Rotate towards mouse
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
}

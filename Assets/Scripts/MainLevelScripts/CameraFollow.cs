using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    private bool clampActive = false;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);

        if (clampActive)
        {
            target.x = Mathf.Clamp(target.x, minBounds.x, maxBounds.x);
            target.y = Mathf.Clamp(target.y, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * Time.deltaTime);
    }

    // =========================
    // ARENA CONTROL
    // =========================
    public void EnableClamp(Vector2 arenaCenter, float arenaWidth, float arenaHeight)
    {
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;

        minBounds = new Vector2(
            arenaCenter.x - arenaWidth / 2f + camWidth / 2f,
            arenaCenter.y - arenaHeight / 2f + camHeight / 2f
        );

        maxBounds = new Vector2(
            arenaCenter.x + arenaWidth / 2f - camWidth / 2f,
            arenaCenter.y + arenaHeight / 2f - camHeight / 2f
        );

        clampActive = true;
    }

    public void DisableClamp()
    {
        clampActive = false;
    }
}

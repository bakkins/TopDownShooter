using UnityEngine;

public class InfiniteGridBackground : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // Player transform

    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    public float parallaxStrength = 0.05f; // How much the background texture moves relative to player

    private Material mat;
    private Vector2 lastPlayerPos;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set on InfiniteGridBackground!");
            enabled = false;
            return;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("No SpriteRenderer found on InfiniteGridBackground!");
            enabled = false;
            return;
        }

        mat = sr.material;

        // Ensure the material texture repeats
        if (!mat.mainTexture.wrapMode.Equals(TextureWrapMode.Repeat))
        {
            mat.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        // Initialize last player position
        lastPlayerPos = player.position;
    }

    void LateUpdate()
    {
        // 1️⃣ Calculate player delta movement
        Vector2 deltaMovement = (Vector2)player.position - lastPlayerPos;

        // 2️⃣ Apply parallax to the texture offset
        mat.mainTextureOffset += deltaMovement * parallaxStrength;

        // 3️⃣ Center the background on the player
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

        // 4️⃣ Store current player position for next frame
        lastPlayerPos = player.position;
    }
}

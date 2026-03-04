using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InfiniteBackground : MonoBehaviour
{
    public Transform player;
    [Range(0f, 0.2f)]
    public float parallaxStrength = 0.05f;

    private Material mat;
    private Vector2 lastPlayerPos;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set!");
            enabled = false;
            return;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mat = sr.material;

        if (mat.mainTexture.wrapMode != TextureWrapMode.Repeat)
            mat.mainTexture.wrapMode = TextureWrapMode.Repeat;

        lastPlayerPos = player.position;
    }

    void LateUpdate()
    {
        Vector2 deltaMovement = (Vector2)player.position - lastPlayerPos;

        // Only move the texture for parallax effect
        mat.mainTextureOffset += deltaMovement * parallaxStrength;

        lastPlayerPos = player.position;
    }
}

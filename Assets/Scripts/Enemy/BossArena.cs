using UnityEngine;
using System.Collections.Generic;

public class BossArena : MonoBehaviour
{
    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public float arenaWidth = 16f;
    public float arenaHeight = 16f;
    public float wallThickness = 1f;

    private List<GameObject> spawnedWalls = new List<GameObject>();

    public void ActivateArena(Enemy boss)
    {
        Vector2 center = boss.transform.position;
        SpawnWalls(center);

        boss.OnDeath += DeactivateArena;
    }

    void SpawnWalls(Vector2 center)
    {
        // Top
        SpawnWall(
            new Vector2(center.x, center.y + arenaHeight / 2f),
            new Vector2(arenaWidth, wallThickness)
        );

        // Bottom
        SpawnWall(
            new Vector2(center.x, center.y - arenaHeight / 2f),
            new Vector2(arenaWidth, wallThickness)
        );

        // Left
        SpawnWall(
            new Vector2(center.x - arenaWidth / 2f, center.y),
            new Vector2(wallThickness, arenaHeight)
        );

        // Right
        SpawnWall(
            new Vector2(center.x + arenaWidth / 2f, center.y),
            new Vector2(wallThickness, arenaHeight)
        );
    }

    void SpawnWall(Vector2 position, Vector2 size)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);

        // Collider
        BoxCollider2D col = wall.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            Debug.LogError("Boss wall prefab missing BoxCollider2D!");
            Destroy(wall);
            return;
        }
        col.size = size;
        col.offset = Vector2.zero;

        // Sprite
        SpriteRenderer sr = wall.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = size;
            sr.sortingOrder = 10; // ensure visible
        }

        spawnedWalls.Add(wall);
    }

    void DeactivateArena()
    {
        foreach (GameObject wall in spawnedWalls)
        {
            if (wall != null)
                Destroy(wall);
        }

        spawnedWalls.Clear();
    }
}

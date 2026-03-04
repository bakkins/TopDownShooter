using System.Collections.Generic;
using UnityEngine;

public class BossArena : MonoBehaviour
{
    [Header("Arena Settings")]
    public GameObject wallPrefab;
    public float arenaWidth = 20f;
    public float arenaHeight = 12f;
    
    [Header("Prefabs")]
    public GameObject bossHealthBarPrefab;

    // These are no longer public because we will find them via code
    private Camera mainCamera; 
    private Canvas uiCanvas; 
    private List<GameObject> spawnedWalls = new List<GameObject>();

public void ActivateArena(Enemy boss)
{
    if (boss == null) return;

    // Find scene references since this is a new clone
    mainCamera = Camera.main; 
    uiCanvas = Object.FindFirstObjectByType<Canvas>(); 

    Vector2 center = boss.transform.position;
    
    ClearWalls();
    SpawnWalls(center); // This will no longer crash

    // Handle Camera Clamping
    if (mainCamera != null)
    {
        CameraFollow follow = mainCamera.GetComponent<CameraFollow>() ?? mainCamera.GetComponentInParent<CameraFollow>();
        follow?.EnableClamp(center, arenaWidth, arenaHeight);
    }

    // Handle UI Spawning - THIS WILL NOW EXECUTE
    if (bossHealthBarPrefab != null && uiCanvas != null)
    {
        GameObject bar = Instantiate(bossHealthBarPrefab, uiCanvas.transform);
        bar.GetComponent<BossHealthBarUI>()?.Initialize(boss);
    }

    boss.OnDeath += DeactivateArena;
}

// Ensure your CreateWall inside SpawnWalls looks like this:
void CreateWall(Vector2 pos, Vector2 scale) {
    if (wallPrefab == null) return;
    // Spawn without parent first to avoid the error
    GameObject w = Instantiate(wallPrefab, pos, Quaternion.identity); 
    w.transform.SetParent(this.transform); // Safe now because 'this' is a clone
    w.transform.localScale = scale;
    spawnedWalls.Add(w);
}

    void SpawnWalls(Vector2 center)
    {
        float left = center.x - (arenaWidth / 2);
        float right = center.x + (arenaWidth / 2);
        float top = center.y + (arenaHeight / 2);
        float bottom = center.y - (arenaHeight / 2);


        CreateWall(new Vector2(center.x, top), new Vector2(arenaWidth, 1));
        CreateWall(new Vector2(center.x, bottom), new Vector2(arenaWidth, 1));
        CreateWall(new Vector2(left, center.y), new Vector2(1, arenaHeight));
        CreateWall(new Vector2(right, center.y), new Vector2(1, arenaHeight));
    }

    public void DeactivateArena()
    {
        ClearWalls();
        if (mainCamera != null)
        {
            CameraFollow follow = mainCamera.GetComponent<CameraFollow>() ?? mainCamera.GetComponentInParent<CameraFollow>();
            follow?.DisableClamp();
        }
        // Destroy the arena clone itself when the boss dies
        Destroy(gameObject, 0.1f); 
    }

    void ClearWalls()
    {
        foreach (GameObject wall in spawnedWalls) if (wall != null) Destroy(wall);
        spawnedWalls.Clear();
    }
}
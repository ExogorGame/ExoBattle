using UnityEngine;

public class DamageSplatSpawner : MonoBehaviour
{
    public static DamageSplatSpawner Instance;
    public DamageSplat splatPrefab;
    public Canvas canvas;

    public Vector2 playerOffset = new Vector2(-50, 0); // left of center
    public Vector2 enemyOffset = new Vector2(50, 0);   // right of center
    public Vector2 basePosition = Vector2.zero;        // center


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Spawn(int damage, Color color, bool isPlayer)
    {
        if (splatPrefab == null || canvas == null) return;

        DamageSplat splat = Instantiate(splatPrefab, canvas.transform, false);

        // Offsets for player vs enemy
        Vector2 playerOffset = new Vector2(-50, 0); // adjust as needed
        Vector2 enemyOffset = new Vector2(50, 0);   // adjust as needed
        Vector2 basePosition = Vector2.zero;        // center of canvas

        Vector2 spawnPos = basePosition + (isPlayer ? playerOffset : enemyOffset);

        // Optional random jitter
        spawnPos += new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 10f));

        splat.GetComponent<RectTransform>().anchoredPosition = spawnPos;
        splat.Initialize(damage, color);
    }

}

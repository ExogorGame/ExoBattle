using UnityEngine;

public class LootLogUI : MonoBehaviour
{
    public static LootLogUI Instance;

    public LootLogEntry entryPrefab;
    public Transform container;

    void Awake()
    {
        Instance = this;
    }

    public void Show(string message)
    {
        LootLogEntry entry = Instantiate(entryPrefab, container);
        entry.SetText(message);
    }
}

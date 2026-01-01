using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public ItemData item;

    [Range(0f, 1f)]
    public float dropChance; // 0.25 = 25%
}

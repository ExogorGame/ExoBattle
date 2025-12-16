using UnityEngine;
using System.Collections.Generic;

public class SoulUpgradeManager : MonoBehaviour
{
    public static SoulUpgradeManager Instance;

    public PlayerCharacter player;

    [Header("Upgrade Settings")]
    public int baseSoulCost = 5;
    public float costMultiplier = 1.5f;
    public int attackPowerPerLevel = 3;

    [Header("Soul Upgrades")]
    public List<SoulUpgradeData> upgrades = new();

    private Dictionary<string, SoulUpgradeData> lookup =
        new Dictionary<string, SoulUpgradeData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var u in upgrades)
            lookup[u.soulId] = u;
    }

    public int GetLevel(string soulId)
    {
        return lookup.ContainsKey(soulId)
            ? lookup[soulId].level
            : 0;
    }

    public int GetCost(string soulId)
    {
        int level = GetLevel(soulId);
        return Mathf.RoundToInt(
            baseSoulCost * Mathf.Pow(costMultiplier, level)
        );
    }

    public bool CanUpgrade(string soulId)
    {
        return player.GetSoulCount(soulId) >= GetCost(soulId);
    }

    public void Upgrade(string soulId)
    {
        if (!lookup.ContainsKey(soulId))
        {
            Debug.LogError($"No upgrade data for soul: {soulId}");
            return;
        }

        int cost = GetCost(soulId);

        if (!CanUpgrade(soulId)) return;

        player.souls[soulId] -= cost;
        lookup[soulId].level++;

        player.attackPower += attackPowerPerLevel;
        player.UpdateStatsUI();

        Debug.Log($"{soulId} upgraded to level {lookup[soulId].level}");
    }
}

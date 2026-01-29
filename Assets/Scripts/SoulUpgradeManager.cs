using UnityEngine;
using System.Collections.Generic;

public class SoulUpgradeManager : MonoBehaviour
{
    public static SoulUpgradeManager Instance;

    public PlayerCharacter player;
    public PlayerUI playerUI;

    [Header("Upgrade Costs Per Level")]
    public int[] soulUpgradeCosts = { 1, 3, 5, 10, 15, 30, 50, 100, 500, 1000, 5000, 10000, 25000 };

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

        // Clamp level so we don't go out of bounds
        if (level >= soulUpgradeCosts.Length)
            return soulUpgradeCosts[soulUpgradeCosts.Length - 1];

        return soulUpgradeCosts[level];
    }


    public bool CanUpgrade(string soulId)
    {
        return player.GetSoulCount(soulId) >= GetCost(soulId);
    }

    private void ApplyUpgrade(SoulUpgradeData data)
    {
        switch (data.upgradeType)
        {
            case SoulUpgradeType.Atk:
                player.baseAttackPower += data.amountPerLevel;
                break;

            case SoulUpgradeType.HP:
                player.baseMaxHealth += data.amountPerLevel;
                player.currentHealth += data.amountPerLevel; // optional
                break;

            case SoulUpgradeType.Def:
                player.baseDefense += data.amountPerLevel;
                break;

            case SoulUpgradeType.Hit:
                player.hitRating += data.amountPerLevel;
                break;

            case SoulUpgradeType.Dodge:
                player.dodgeRating += data.amountPerLevel;
                break;

            case SoulUpgradeType.AtkPercent:
                player.atkPercent += data.amountPerLevel;
                break;

            case SoulUpgradeType.HPPercent:
                player.hpPercent += data.amountPerLevel;
                break;

            case SoulUpgradeType.DefPercent:
                player.defPercent += data.amountPerLevel;
                break;

            case SoulUpgradeType.CritChance:
                player.critChance += data.amountPerLevel / 100f;
                break;

            case SoulUpgradeType.CritDamage:
                player.critDamage += data.amountPerLevel / 100f;
                break;
        }
        player.RecalculateStats();

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

        ApplyUpgrade(lookup[soulId]);
        player.UpdateUI();

        Debug.Log($"{soulId} upgraded to level {lookup[soulId].level}");
    }
}

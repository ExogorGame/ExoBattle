using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Idle RPG/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    public TierName[] tierNames;


    [Header("Main Stat")]
    public ScalableStat mainStat;


    [Header("Possible Substats")]
    public ScalableStat[] possibleSubstats;

    [Header("Substat Rolls")]
    public int minSubstats = 0;
    public int maxSubstats = 3;

    // Create an instance of this item for a given tier
    public ItemInstance CreateInstance(int itemTier)
    {
        ItemInstance item = new ItemInstance();
        item.itemData = this;

        // --- Assign tiered name ---
        var tierName = tierNames.FirstOrDefault(t => t.tier == itemTier);
        if (tierName != null)
            item.itemData.itemName = tierName.name;

        // --- Roll main stat ---
        RollStat(item, mainStat, itemTier, true);

        // --- Prepare substat pool ---
        // Use only possibleSubstats — do NOT add mainStat
        List<ScalableStat> pool = new List<ScalableStat>(possibleSubstats);

        int substatCount = Random.Range(minSubstats, maxSubstats + 1);

        for (int i = 0; i < substatCount; i++)
        {
            int index = Random.Range(0, pool.Count);
            RollStat(item, pool[index], itemTier, false);
        }

        return item;
    }

    void RollStat(ItemInstance item, ScalableStat stat, int tier, bool isMain = false)
    {
        tier = Mathf.Clamp(tier - 1, 0, stat.tiers.Length - 1);
        AffixTier range = stat.tiers[tier];

        int value = Random.Range(range.min, range.max + 1);

        // Store roll + range
        item.rolls.Add(new StatRoll
        {
            stat = stat.stat,
            value = value,
            min = range.min,
            max = range.max,
            isMainStat = isMain
        });

        // Apply totals
        switch (stat.stat)
        {
            case StatType.Attack: item.attack += value; break;
            case StatType.Defense: item.defense += value; break;
            case StatType.MaxHealth: item.maxHealth += value; break;
            case StatType.HitRating: item.hitRating += value; break;
            case StatType.DodgeRating: item.dodgeRating += value; break;
            case StatType.AttackPercent: item.atkPercent += value; break;
            case StatType.DefPercent: item.defPercent += value; break;
            case StatType.HPPercent: item.hpPercent += value; break;
            case StatType.CritChance: item.critChance += value; break;
            case StatType.CritDamage: item.critDamage += value; break;
        }
    }


}



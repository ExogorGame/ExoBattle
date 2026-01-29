using UnityEngine;
public enum ItemType
{
    Weapon,
    Helmet,
    Body,
    Legs,
    Gloves,
    Boots,
}

public enum StatType
{
    Attack,
    Defense,
    MaxHealth,
    HitRating,
    DodgeRating,
    AttackPercent,
    DefPercent,
    HPPercent,
    CritChance,
    CritDamage

}

[System.Serializable]
public class StatRange
{
    public StatType stat;
    public int min;
    public int max;
}

[System.Serializable]
public class AffixTier
{
    public int min;
    public int max;
}

[System.Serializable]
public class ScalableStat
{
    public StatType stat;      // Attack, Defense, HP, etc
    public AffixTier[] tiers;  // Tier 0 = Tier 1, Tier 1 = Tier 2, etc
}

[System.Serializable]
public class TierName
{
    public int tier;       // The tier this name corresponds to
    public string name;    // The item name for this tier
}




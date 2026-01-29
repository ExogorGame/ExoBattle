using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData itemData;

    public int attack;
    public int defense;
    public int maxHealth;
    public int hitRating;
    public int dodgeRating;
    public float atkPercent;
    public float defPercent;
    public float hpPercent;
    public float critChance;
    public float critDamage;


    public List<StatRoll> rolls = new List<StatRoll>();
}

[System.Serializable]
public class StatRoll
{
    public StatType stat;
    public int value;
    public int min;
    public int max;
    public bool isMainStat;
}


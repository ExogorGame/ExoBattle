using UnityEngine;

[System.Serializable]
public class Talent
{
    public string name;
    public int maxLevel;
    public int currentLevel;

    // Tier system
    public int tier = 1;

    // Prerequisite talent
    public string requiredTalent;
    public int requiredLevel;

    public bool IsMaxed => currentLevel >= maxLevel;

    public bool IsUnlocked(PlayerCharacter player)
    {
        if (string.IsNullOrEmpty(requiredTalent))
            return true; // Tier 1 talents always unlocked

        Talent req = player.GetTalent(requiredTalent);
        return req != null && req.currentLevel >= requiredLevel;
    }

    public void Upgrade(PlayerCharacter player)
    {
        if (IsMaxed || !IsUnlocked(player))
            return;

        currentLevel++;

        switch (name)
        {
            case "+5 HP":
                player.baseMaxHealth += 5;
                player.currentHealth += 5;
                break;
            case "+1 Atk":
                player.baseAttackPower++;
                break;
            case "+1 Def":
                player.baseDefense++;
                break;
            case "+10% HP":
                player.hpPercent += 10f;
                break;
            case "+5% Atk":
                player.atkPercent += 5f;
                break;
            case "+5% Def":
                player.defPercent += 5f;
                break;
            case "+1% Crit Chance":
                player.critChance += 0.01f; // +1%
                break;
            case "+10% CritDamage":
                player.critDamage += 0.1f; // +10%
                break;
            case "+5% True Dmg per HP":
                player.trueDamageHPScaling += 5f; // 5% per talent level
                break;
            case "+5% Def->Atk":
                player.defToAtkPercent += 5f; // 5% of defense converted per level
                break;
            case "+30 HP":
                player.baseMaxHealth += 30;
                player.currentHealth += 30;
                break;
            case "+5 Atk":
                player.baseAttackPower += 5;
                break;
            case "+5 Def":
                player.baseDefense += 5;
                break;
        }
        player.RecalculateStats();
    }
}



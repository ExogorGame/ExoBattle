using UnityEngine;

public static class CombatMath
{
    public static bool AttackHits(int attackerHit, int defenderDodge)
    {
        int difference = defenderDodge - attackerHit;

        if (difference <= 0)
            return true; // guaranteed hit

        float missChance = difference / 100f;

        return Random.value >= missChance;
    }
}

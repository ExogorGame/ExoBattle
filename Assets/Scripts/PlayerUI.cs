using System.Drawing;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    public void UpdateUI(PlayerCharacter player)
    {
        statsText.text =
    $"<color=green>Hp: {player.currentHealth}/{player.maxHealth}</color>\n" +
    $"<color=red>Atk: {player.attackPower}</color>\t" +
    $"<color=blue>Def: {player.defense}</color>\n" +
    $"<color=white>Hit: {player.hitRating}</color>\t" +
    $"<color=yellow>Dodge: {player.dodgeRating}</color>\n" +
    $"<color=yellow>Crit: {player.critChance:F1}%\t" +
    $"<color=yellow>CDmg: +{player.critDamage:F0}%</color>\n";

        levelText.text = $"Lvl: {player.combatLevel}";
        xpText.text = $"XP: {player.currentXP}/{player.xpToNextLevel}";
    }
}

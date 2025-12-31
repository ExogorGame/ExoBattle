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
    $"<color=green>HP: {player.currentHealth}/{player.maxHealth}</color>\n" +
    $"<color=red>ATK: {player.attackPower}</color>\n" +
    $"<color=yellow>DEF: {player.defense}</color>";


        levelText.text = $"Lvl: {player.combatLevel}";
        xpText.text = $"XP: {player.currentXP}/{player.xpToNextLevel}";
    }
}

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
            $"HP: {player.currentHealth}/{player.maxHealth}\n" +
            $"ATK: {player.attackPower}\n" +
            $"DEF: {player.defense}";

        levelText.text = $"Lvl: {player.combatLevel}";
        xpText.text = $"XP: {player.currentXP}/{player.xpToNextLevel}";
    }
}

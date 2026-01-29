using UnityEngine;
using TMPro;

public class EnemyUIManager : MonoBehaviour
{
    public TextMeshProUGUI statsText; // Assign the panel's Text
    private EnemyCharacter currentEnemy;

    public void SetEnemy(EnemyCharacter enemy)
    {
        currentEnemy = enemy;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (currentEnemy == null || statsText == null) return;

        statsText.text =
        $"<color=green>HP: {currentEnemy.currentHealth}/{currentEnemy.maxHealth}</color>\n" +
        $"<color=red>ATK: {currentEnemy.attackPower}</color>\t" +
        $"<color=blue>DEF: {currentEnemy.defense}</color>\n" +
        $"<color=white>HIT: {currentEnemy.hitRating}</color>\t" +
        $"<color=yellow>DODGE: {currentEnemy.dodgeRating}</color>\n" +
        $"XP: {currentEnemy.xpPerEnemy}\n";
    }

    void Update()
    {
        if (currentEnemy != null)
        {
            UpdateUI(); // refresh every frame if health changes
        }
    }
}

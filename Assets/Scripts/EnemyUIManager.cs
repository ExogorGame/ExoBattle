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

        statsText.text = $"HP: {currentEnemy.currentHealth}/{currentEnemy.maxHealth}\n" +
                         $"ATK: {currentEnemy.attackPower}\n" +
                         $"DEF: {currentEnemy.defense}\n" +
                         $"XP: {currentEnemy.xpPerEnemy}\n" +
                         $"Coins: {currentEnemy.coinsDrop}\n" +
                         $"Bones: {currentEnemy.bonesDrop}";
    }

    void Update()
    {
        if (currentEnemy != null)
        {
            UpdateUI(); // refresh every frame if health changes
        }
    }
}

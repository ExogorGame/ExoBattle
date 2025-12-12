using UnityEngine;
using TMPro;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int attackPower = 20;
    public int defense = 5;
    public int currentHealth;

    [Header("Combat Level")]
    public int combatLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int xpPerEnemy = 50;

    [Header("Loot")]
    public int coins = 0;
    public int bones = 0;

    [Header("Loot Settings")]
    public int coinsDrop = 10; // amount of coins given when killed
    public int bonesDrop = 1; // amount of bones per kill

    [Header("UI")]
    public TextMeshProUGUI statsText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateStatsUI();
    }

    // Deal damage
    public void Attack(PlayerCharacter target)
    {
        if (target == null) return;

        int finalDamage = Mathf.Max(attackPower - target.defense, 0);
        target.TakeDamage(finalDamage);

        Debug.Log($"{gameObject.name} attacks {target.gameObject.name} for {finalDamage} damage.");
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateStatsUI();

        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    // Reset health for respawn
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateStatsUI();
        Debug.Log($"{gameObject.name} has respawned!");
    }

    // Called when this character dies
    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
    }

    // Gain XP
    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"{gameObject.name} gained {amount} XP. Current XP: {currentXP}/{xpToNextLevel}");

        // Check for level up
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        UpdateStatsUI();
    }

    void LevelUp()
    {
        combatLevel++;
        attackPower += 5;  // Example scaling
        defense += 2;      // Example scaling
        maxHealth += 20;
        currentHealth = maxHealth;

        Debug.Log($"{gameObject.name} leveled up! Combat Level: {combatLevel}");
    }

    public void GainCoins(int amount)
    {
        coins += amount;
        Debug.Log($"{gameObject.name} gained {amount} Coins.");

        UpdateStatsUI();
    }

    public void GainBones(int amount)
    {
        bones += amount;
        Debug.Log($"{gameObject.name} gained {amount} Bones.");

        UpdateStatsUI();
    }

    void UpdateStatsUI()
    {
        if (statsText != null)
        {
            statsText.text = $"HP: {currentHealth}/{maxHealth}\n" +
                             $"ATK: {attackPower}\n" +
                             $"DEF: {defense}\n" +
                             $"Level: {combatLevel}\n" +
                             $"XP: {currentXP}/{xpToNextLevel}" +
                             $"Coins: {coins}" +
                             $"Bones: {bones}";


        }
    }
}

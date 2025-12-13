using UnityEngine;
using TMPro;

public class PlayerCharacter : MonoBehaviour
{
    public EnemyCharacter enemy;

    [Header("Stats")]
    public int maxHealth = 100;
    public int attackPower = 20;
    public int defense = 5;
    public int currentHealth;

    [Header("Combat Level")]
    public int combatLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    

    [Header("Loot")]
    public int coins = 0;
    public int bones = 0;

    [Header("UI")]
    public TextMeshProUGUI statsText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateStatsUI();
    }

    // Deal damage
    public void Attack(EnemyCharacter target)
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

        DamageSplatSpawner.Instance.Spawn(damage, Color.yellow, true);

        if (currentHealth <= 0)
            Die();
    }

    // Called when this character dies
    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
    }

    // Gain XP
    public void GainLoot(int xpAmount, int coinsAmount, int bonesAmount)
    {
        currentXP += xpAmount;
        Debug.Log($"{gameObject.name} gained {xpAmount} XP. Current XP: {currentXP}/{xpToNextLevel}");

        // Check for level up
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        coins += coinsAmount;
        Debug.Log($"{gameObject.name} gained {coinsAmount} Coins.");

        bones += bonesAmount;
        Debug.Log($"{gameObject.name} gained {bonesAmount} Bones.");

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

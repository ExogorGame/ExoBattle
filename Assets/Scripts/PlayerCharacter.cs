using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerCharacter : MonoBehaviour
{
    public EnemyCharacter enemy;
    public PlayerUI playerUI;

    [Header("Stats")]
    public int maxHealth = 10;
    public int attackPower = 1;
    public int defense = 5;
    public int currentHealth;

    [Header("Combat Level")]
    public int combatLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    

    [Header("Loot")]
    public Dictionary<string, int> souls = new();

    [Header("UI")]
    public TextMeshProUGUI statsText;

    void Start()
    {
        currentHealth = maxHealth;
        playerUI.UpdateUI(this);
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
        UpdateUI();

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
    public void GainLoot(int xpAmount)
    {
        currentXP += xpAmount;
        Debug.Log($"{gameObject.name} gained {xpAmount} XP. Current XP: {currentXP}/{xpToNextLevel}");

        // Check for level up
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
        UpdateUI();
    }

    public void AddSoul(string soulId, int amount = 1)
    {
        if (!souls.ContainsKey(soulId))
            souls[soulId] = 0;

        souls[soulId] += amount;

        Debug.Log($"Gained {soulId} soul! Total: {souls[soulId]}");

    }

    public int GetSoulCount(string soulId)
    {
        return souls.ContainsKey(soulId) ? souls[soulId] : 0;
    }



    void LevelUp()
    {
        combatLevel++;
        attackPower += 1;
        defense += 1;
        maxHealth += 5;
        currentHealth = maxHealth;

        Debug.Log($"{gameObject.name} leveled up! Combat Level: {combatLevel}");
    }


    public void UpdateUI()
    {
        if (playerUI != null)
            playerUI.UpdateUI(this);
    }
}

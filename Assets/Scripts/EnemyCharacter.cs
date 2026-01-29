using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EnemyCharacter : MonoBehaviour
{
    public PlayerCharacter player;
    public EnemyCharacter enemy;

    [Header("Item Drops")]
    public List<LootEntry> lootTable = new();

    [Header("Tier Settings")]
    [Tooltip("Determines the tier of items this enemy drops.")]
    public int itemTier = 1; // 1 = first zone, 2 = second zone, etc



    [Header("Stats")]
    public int maxHealth = 100;
    public int attackPower = 20;
    public int defense = 5;
    public int hitRating = 1;
    public int dodgeRating = 1;
    public int currentHealth;


    [Header("Loot Settings")]
    public int xpPerEnemy = 50;
    public int souls = 1;
    public string soulId;

    [Header("UI")]
    public TextMeshProUGUI statsText;

    public void Start()
    {
        currentHealth = maxHealth;
        UpdateStatsUI();
    }

    public void Attack(PlayerCharacter target)
    {
        if (target == null) return;

        if (!CombatMath.AttackHits(hitRating, target.dodgeRating))
        {
            Debug.Log($"{name} missed {target.name}!");
            DamageSplatSpawner.Instance.Spawn(0, Color.gray, true);
            return;
        }

        int finalDamage = Mathf.Max(attackPower - target.defense, 0);
        target.TakeDamage(finalDamage);

        Debug.Log($"{name} hit {target.name} for {finalDamage} damage.");

    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateStatsUI();

        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        DamageSplatSpawner.Instance.Spawn(damage, Color.red, false);

        if (currentHealth <= 0)
            Die();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateStatsUI();
        Debug.Log($"{gameObject.name} has respawned!");
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
    }
    public void UpdateStatsUI()
    {
        if (statsText != null)
        {
            statsText.text = $"HP: {currentHealth}/{maxHealth}\n" +
                             $"ATK: {attackPower}\n" +
                             $"DEF: {defense}\n" +
                             $"XP: {xpPerEnemy}\n";
        }
    }

    public ItemInstance TryDropItem()
    {
        if (lootTable == null || lootTable.Count == 0)
            return null;

        float totalChance = 0f;

        foreach (var entry in lootTable)
            totalChance += entry.dropChance;

        float roll = Random.value;

        if (roll > totalChance)
            return null;

        float cumulative = 0f;

        foreach (var entry in lootTable)
        {
            cumulative += entry.dropChance;
            if (roll <= cumulative)
            {
                return entry.item.CreateInstance(itemTier);
            }
        }
        return null;
    }
}


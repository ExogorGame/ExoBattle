using UnityEngine;
using TMPro;

public class EnemyCharacter : MonoBehaviour
{
    public PlayerCharacter player;
    public EnemyCharacter enemy;

    [Header("Stats")]
    public int maxHealth = 100;
    public int attackPower = 20;
    public int defense = 5;
    public int currentHealth;


    [Header("Loot Settings")]
    public int xpPerEnemy = 50;
    public int coinsDrop = 10; 
    public int bonesDrop = 1;

    [Header("UI")]
    public TextMeshProUGUI statsText;

    public void Start()
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
    public void UpdateStatsUI()
    {
        if (statsText != null)
        {
            statsText.text = $"HP: {currentHealth}/{maxHealth}\n" +
                             $"ATK: {attackPower}\n" +
                             $"DEF: {defense}\n" +
                             $"XP: {xpPerEnemy}\n" +
                             $"Coins: {coinsDrop}\n" +
                             $"Bones: {bonesDrop}";
        }
    }
 }


using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Inventory")]
    public List<ItemInstance> inventory = new List<ItemInstance>();
    public int maxInventorySlots = 32;

    public ItemInstance equippedWeapon;
    public ItemInstance equippedArmour;

    public EquipmentSlotUI weaponUI;
    public EquipmentSlotUI armourUI;

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
    public PlayerUI playerUI;
    public TextMeshProUGUI statsText;

    void Start()
    {
        currentHealth = maxHealth;
        weaponUI?.SetItem(null, this);
        armourUI?.SetItem(null, this);
        UpdateUI();
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
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateUI();
        DamageSplatSpawner.Instance.Spawn(damage, Color.yellow, true);

        if (currentHealth <= 0) Die();
    }

    void Die() => Debug.Log($"{gameObject.name} has died!");

    // Gain XP
    public void GainLoot(int xpAmount)
    {
        currentXP += xpAmount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
        UpdateUI();
    }

    public void AddSoul(string soulId, int amount = 1)
    {
        if (!souls.ContainsKey(soulId)) souls[soulId] = 0;
        souls[soulId] += amount;
    }

    public int GetSoulCount(string soulId) => souls.ContainsKey(soulId) ? souls[soulId] : 0;

    void LevelUp()
    {
        combatLevel++;
        attackPower++;
        defense++;
        maxHealth += 5;
        currentHealth = maxHealth;
    }

    public void UpdateUI()
    {
        playerUI?.UpdateUI(this);
    }

    // Equip an item and update stats & equipment slots
    public void EquipItem(ItemInstance item)
    {
        if (item.itemData.itemType == ItemType.Weapon)
        {
            if (equippedWeapon != null) attackPower -= equippedWeapon.attack;
            equippedWeapon = item;
            attackPower += item.attack;
            weaponUI?.SetItem(equippedWeapon, this);
        }
        else if (item.itemData.itemType == ItemType.Armour)
        {
            if (equippedArmour != null) defense -= equippedArmour.defense;
            equippedArmour = item;
            defense += item.defense;
            armourUI?.SetItem(equippedArmour, this);
        }

        UpdateUI();
    }

    // Add an item to inventory (returns false if full)
    public bool AddItem(ItemInstance item)
    {
        if (inventory.Count >= maxInventorySlots)
        {
            Debug.Log("Inventory full! Cannot add item: " + item.itemData.itemName);
            return false;
        }

        inventory.Add(item);
        return true;
    }

    // Remove item from inventory
    public void RemoveItem(ItemInstance item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
        }
    }
}

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Inventory")]
    public List<ItemInstance> inventory = new List<ItemInstance>();
    public int maxInventorySlots = 64;

    public ItemInstance equippedWeapon;
    public ItemInstance equippedHelmet;
    public ItemInstance equippedBody;
    public ItemInstance equippedLegs;
    public ItemInstance equippedGloves;
    public ItemInstance equippedBoots;


    public EquipmentSlotUI weaponUI;
    public EquipmentSlotUI helmetUI;
    public EquipmentSlotUI bodyUI;
    public EquipmentSlotUI legsUI;
    public EquipmentSlotUI glovesUI;
    public EquipmentSlotUI bootsUI;

    [Header("Stats")]
    public int maxHealth = 10;
    public int attackPower = 1;
    public int defense = 5;
    public int hitRating = 1;
    public int dodgeRating = 1;
    public int currentHealth;

    [Header("Combat Level")]
    public int combatLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("Loot")]
    public Dictionary<string, int> souls = new();

    [Header("UI")]
    public PlayerUI playerUI;
    public InventoryUI inventoryUI;
    public TextMeshProUGUI statsText;

    void Start()
    {
        currentHealth = maxHealth;
        weaponUI?.SetItem(null, this);
        helmetUI?.SetItem(null, this);
        bodyUI?.SetItem(null, this);
        legsUI?.SetItem(null, this);
        glovesUI?.SetItem(null, this);
        bootsUI?.SetItem(null, this);
        UpdateUI();
    }
    public void Attack(EnemyCharacter target)
    {
        if (target == null) return;

        if (!CombatMath.AttackHits(hitRating, target.dodgeRating))
        {
            Debug.Log($"{name} missed {target.name}!");
            DamageSplatSpawner.Instance.Spawn(0, Color.gray, false);
            return;
        }

        int finalDamage = Mathf.Max(attackPower - target.defense, 0);
        target.TakeDamage(finalDamage);

        Debug.Log($"{name} hit {target.name} for {finalDamage} damage.");
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

    public void EquipItem(ItemInstance newItem)
    {
        if (newItem == null || newItem.itemData == null)
        {
            Debug.LogError("Cannot equip null item or item with null itemData!");
            return;
        }

        switch (newItem.itemData.itemType)
        {
            case ItemType.Weapon:
                SetEquippedItem(ref equippedWeapon, newItem, weaponUI);
                break;

            case ItemType.Helmet:
                SetEquippedItem(ref equippedHelmet, newItem, helmetUI);
                break;

            case ItemType.Body:
                SetEquippedItem(ref equippedBody, newItem, bodyUI);
                break;

            case ItemType.Legs:
                SetEquippedItem(ref equippedLegs, newItem, legsUI);
                break;

            case ItemType.Gloves:
                SetEquippedItem(ref equippedGloves, newItem, glovesUI);
                break;

            case ItemType.Boots:
                SetEquippedItem(ref equippedBoots, newItem, bootsUI);
                break;
        }

        UpdateUI();
        InventoryUI inventoryUI = Object.FindFirstObjectByType<InventoryUI>();
        inventoryUI?.Refresh();
    }

    private void SetEquippedItem(ref ItemInstance slotItem, ItemInstance newItem, EquipmentSlotUI slotUI)
    {
        // Remove stats of previous item in that slot
        if (slotItem != null)
            ApplyItemStats(slotItem, -1);

        slotItem = newItem;
        ApplyItemStats(newItem, +1);

        slotUI?.SetItem(slotItem, this);
    }

    private void ApplyItemStats(ItemInstance item, int sign)
    {
        if (item == null || item.itemData == null) return;

        attackPower += item.attack * sign;
        defense += item.defense * sign;
        maxHealth += item.maxHealth * sign;
        hitRating += item.hitRating * sign;
        dodgeRating += item.dodgeRating * sign;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // ========================= INVENTORY MANAGEMENT =========================

    public bool AddItem(ItemInstance item)
    {
        if (inventory.Count >= maxInventorySlots)
        {
            Debug.LogWarning($"Inventory full! Cannot add {item?.itemData?.itemName}");
            return false;
        }

        if (item != null && item.itemData != null)
            inventory.Add(item);

        return true;
    }

    public void RemoveItem(ItemInstance item)
    {
        if (item == null) return;
        inventory.Remove(item);
    }
}

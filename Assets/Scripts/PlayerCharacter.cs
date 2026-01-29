using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Inventory")]
    public List<ItemInstance> inventory = new List<ItemInstance>();
    public int maxInventorySlots = 80;

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

    [Header("BaseStats")]
    public int baseMaxHealth = 10;
    public int baseAttackPower = 1;
    public int baseDefense = 1;
    public int hitRating = 1;
    public int dodgeRating = 1;
    public int currentHealth;
    public float critChance = 5f;
    public float critDamage = 50f;

    [Header("Scaling Stats")]
    public float hpPercent = 0f;   // +10 = +10%
    public float atkPercent = 0f;
    public float defPercent = 0f;

    [Header("Talent Stats")]
    public float trueDamageHPScaling = 0f; // % of max HP as true damage
    public float defToAtkPercent = 0f;    // % of defense converted to attack


    [Header("Final Stats (Calculated)")]
    public int maxHealth;
    public int attackPower;
    public int defense;


    [Header("Combat Level")]
    public int combatLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int baseXP = 100;
    public float xpGrowthRate = 1.25f;

    [Header("Talents")]
    public int availableTalentPoints = 0;
    public Talent[] talents;


    [Header("Loot")]
    public Dictionary<string, int> souls = new();

    [Header("UI")]
    public PlayerUI playerUI;
    public InventoryUI inventoryUI;
    public TextMeshProUGUI statsText;


    void Start()
    {
        UpdateXPToNextLevel();
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

        int baseDamage = Mathf.Max(attackPower - target.defense, 0);

        bool isCrit = CombatMath.RollCrit(critChance / 100f);
        int finalDamage = baseDamage;

        if (isCrit)
            finalDamage = Mathf.RoundToInt(baseDamage * (1f + critDamage / 100f));

        int trueDamage = Mathf.RoundToInt(maxHealth * (trueDamageHPScaling / 100f));

        target.TakeDamage(finalDamage + trueDamage);

    }



    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateUI();
        DamageSplatSpawner.Instance.Spawn(damage, Color.yellow, true);

        if (currentHealth <= 0) Die();
    }

    public bool isDead {  get; private set; }
    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} has died!");

        // Show the Death UI via UIManager
        UIManager uiManager = Object.FindFirstObjectByType<UIManager>();
        uiManager?.ShowDeathUI();
    }


    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateXPToNextLevel()
    {
        xpToNextLevel = Mathf.RoundToInt(baseXP * Mathf.Pow(xpGrowthRate, combatLevel - 1));
    }


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
        baseAttackPower++;
        baseDefense++;
        baseMaxHealth += 5;
        currentHealth = maxHealth;

        availableTalentPoints++;

        UpdateXPToNextLevel();

        RecalculateStats();
        UpdateUI();
        FindFirstObjectByType<TalentUI>()?.Refresh();

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

        baseAttackPower += item.attack * sign;
        baseDefense += item.defense * sign;
        baseMaxHealth += item.maxHealth * sign;
        hitRating += item.hitRating * sign;
        dodgeRating += item.dodgeRating * sign;
        atkPercent += item.atkPercent * sign;
        defPercent += item.defPercent * sign;
        hpPercent += item.hpPercent * sign;
        critChance += item.critChance * sign;
        critDamage += item.critDamage * sign;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        RecalculateStats();
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

    public Talent GetTalent(string talentName)
    {
        foreach (var t in talents)
        {
            if (t.name == talentName)
                return t;
        }
        return null;
    }

    public bool SpendTalentPoint(string talentName)
    {
        if (availableTalentPoints <= 0)
            return false;

        Talent talent = GetTalent(talentName);
        if (talent == null || talent.IsMaxed || !talent.IsUnlocked(this))
            return false;

        talent.Upgrade(this);
        availableTalentPoints--;
        UpdateUI();
        return true;
    }

    public void RecalculateStats()
    {
        maxHealth = Mathf.RoundToInt(baseMaxHealth * (1f + hpPercent / 100f));
        defense = Mathf.RoundToInt(baseDefense * (1f + defPercent / 100f));

        // Convert defense into attack BEFORE attack scaling
        int convertedAtk = Mathf.RoundToInt(defense * (defToAtkPercent / 100f));
        attackPower = baseAttackPower + convertedAtk;
        attackPower = Mathf.RoundToInt(attackPower * (1f + atkPercent / 100f));

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

}

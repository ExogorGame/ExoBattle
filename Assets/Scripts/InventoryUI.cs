using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public PlayerCharacter player;
    public InventorySlotUI slotPrefab;
    public Transform gridParent;

    [Header("Grid Settings")]
    public int columns = 4;
    public int rows = 8;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();
    private int totalSlots => columns * rows;

    void Start()
    {
        CreateSlots();
        Refresh();
    }

    /// <summary>
    /// Instantiate the fixed grid of inventory slots
    /// </summary>
    void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, gridParent);
            slots.Add(slot);
        }
    }

    /// <summary>
    /// Refresh the inventory UI to match the player's current inventory
    /// </summary>
    public void Refresh()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ItemInstance item = (i < player.inventory.Count) ? player.inventory[i] : null;
            slots[i].SetItem(item, player);
        }
    }

    /// <summary>
    /// Try to add an item to the inventory and refresh UI automatically
    /// </summary>
    public bool AddItem(ItemInstance item)
    {
        if (player.inventory.Count >= player.maxInventorySlots)
        {
            Debug.LogWarning($"Inventory full! Cannot add {item.itemData.itemName}");
            return false;
        }

        player.inventory.Add(item);
        Refresh();
        return true;
    }

    /// <summary>
    /// Remove an item from the inventory and refresh UI
    /// </summary>
    public void RemoveItem(ItemInstance item)
    {
        if (player.inventory.Contains(item))
        {
            player.inventory.Remove(item);
            Refresh();
        }
    }

    /// <summary>
    /// Optional: clear all inventory slots visually
    /// </summary>
    public void Clear()
    {
        foreach (var slot in slots)
            slot.SetItem(null, player);
    }
}


using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public PlayerCharacter player;
    public InventorySlotUI slotPrefab;
    public Transform gridParent;

    [Header("Grid Settings")]
    public int columns = 8;
    public int rows = 8;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();
    private int totalSlots => columns * rows;

    void Start()
    {
        CreateSlots();
        Refresh();
    }

    void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, gridParent);
            slots.Add(slot);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ItemInstance item = (i < player.inventory.Count) ? player.inventory[i] : null;
            slots[i].SetItem(item, player, this);
        }
    }
}



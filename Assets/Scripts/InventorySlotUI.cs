using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;

    private ItemInstance item;
    private PlayerCharacter player;
    private InventoryUI inventoryUI;

    public void SetItem(ItemInstance newItem, PlayerCharacter playerCharacter, InventoryUI invUI = null)
    {
        item = newItem;
        player = playerCharacter;
        inventoryUI = invUI;

        if (icon == null) return;

        if (item == null || item.itemData == null)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = item.itemData.icon;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || player == null) return;
        Vector2 offset = new Vector2(16f, -16f);
        TooltipUI.Instance.Show(item, player, eventData.position + offset, canEquip: true, inventoryUI);
    }
}




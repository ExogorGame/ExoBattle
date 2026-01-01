using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    private ItemInstance equippedItem;
    private PlayerCharacter player;

    public void SetItem(ItemInstance item, PlayerCharacter playerCharacter)
    {
        equippedItem = item;
        player = playerCharacter;

        if (equippedItem == null)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = equippedItem.itemData.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (equippedItem == null || player == null) return;

        Vector2 offset = new Vector2(16f, -16f);
        TooltipUI.Instance.Show(equippedItem, player, eventData.position + offset, canEquip: false);
    }
}

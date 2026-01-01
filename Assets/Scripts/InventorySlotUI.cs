using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI statsText;

    private ItemInstance item;
    private PlayerCharacter player;

    public void SetItem(ItemInstance newItem, PlayerCharacter playerCharacter)
    {
        item = newItem;
        player = playerCharacter;

        if (item == null)
        {
            icon.enabled = false;
            statsText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = item.itemData.icon;
        statsText.text = ""; // optional quantity/stack
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || player == null) return;

        Vector2 offset = new Vector2(16f, -16f);
        TooltipUI.Instance.Show(item, player, eventData.position + offset, canEquip: true);
    }
}



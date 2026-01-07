using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statsText;
    public Button equipButton;
    public Button destroyButton;

    private ItemInstance item;
    private PlayerCharacter player;
    private InventoryUI inventoryUI;

    public void Setup(ItemInstance itemInstance, PlayerCharacter playerCharacter, InventoryUI invUI)
    {
        item = itemInstance;
        player = playerCharacter;
        inventoryUI = invUI;

        nameText.text = item.itemData.itemName;
        statsText.text = $"ATK: {item.attack}\nDEF: {item.defense}";
        icon.sprite = item.itemData.icon;

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(Equip);

        destroyButton.onClick.RemoveAllListeners();
        destroyButton.onClick.AddListener(DestroyItem);
    }

    void Equip()
    {
        player.EquipItem(item);
        inventoryUI?.Refresh();
    }

    void DestroyItem()
    {
        if (item != null)
        {
            player.RemoveItem(item); 
            inventoryUI.Refresh();   
            item = null;             
        }
    }
}

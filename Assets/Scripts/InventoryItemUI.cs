using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statsText;
    public Button equipButton;

    private ItemInstance item;
    private PlayerCharacter player;

    public void Setup(ItemInstance itemInstance, PlayerCharacter playerCharacter)
    {
        item = itemInstance;
        player = playerCharacter;

        nameText.text = item.itemData.itemName;
        statsText.text = $"ATK: {item.attack}\nDEF: {item.defense}";
        icon.sprite = item.itemData.icon;

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(Equip);
    }

    void Equip()
    {
        player.EquipItem(item);
    }
}

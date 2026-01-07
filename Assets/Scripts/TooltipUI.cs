using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public TextMeshProUGUI tooltipText;
    public CanvasGroup canvasGroup;
    public Button equipButton;
    public Button destroyButton;

    private ItemInstance currentItem;
    private PlayerCharacter currentPlayer;
    private InventoryUI currentInventoryUI;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(ItemInstance item, PlayerCharacter player, Vector2 position, bool canEquip, InventoryUI inventoryUI = null)
    {
        currentItem = item;
        currentPlayer = player;
        currentInventoryUI = inventoryUI;


        equipButton.gameObject.SetActive(canEquip);
        destroyButton.gameObject.SetActive(item != null);

        if (item == null || item.itemData == null)
        {
            tooltipText.text = "";
            return;
        }

        string stats = "";
        if (item.attack != 0) stats += $"ATK: {item.attack}\n";
        if (item.defense != 0) stats += $"DEF: {item.defense}\n";
        if (item.maxHealth != 0) stats += $"HP: {item.maxHealth}\n";
        if (item.hitRating != 0) stats += $"HIT: {item.hitRating}\n";
        if (item.dodgeRating != 0) stats += $"DODGE: {item.dodgeRating}\n";

        tooltipText.text = $"<b>{item.itemData.itemName}</b>\n{stats.TrimEnd()}";

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(OnEquipClicked);

        destroyButton.onClick.RemoveAllListeners();
        destroyButton.onClick.AddListener(OnDestroyClicked);

        transform.position = position;
        canvasGroup.alpha = 1;
    }




    public void Hide()
    {
        canvasGroup.alpha = 0;
        currentItem = null;
        currentPlayer = null;
        currentInventoryUI = null;
    }

    private void OnEquipClicked()
    {
        if (currentItem != null && currentPlayer != null)
        {
            currentPlayer.EquipItem(currentItem);
            currentInventoryUI?.Refresh();
            Hide();
        }
    }

    private void OnDestroyClicked()
    {
        if (currentItem != null && currentPlayer != null)
        {
            currentPlayer.RemoveItem(currentItem); // Remove from inventory
            currentInventoryUI?.Refresh();          // Refresh UI
            Hide();
        }
    }
}


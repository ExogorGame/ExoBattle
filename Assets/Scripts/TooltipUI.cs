using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI mainStatText;
    public TextMeshProUGUI[] substatTexts;

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

    public void Show(ItemInstance item, PlayerCharacter player, bool canEquip, InventoryUI inventoryUI = null)
    {
        currentItem = item;
        currentPlayer = player;
        currentInventoryUI = inventoryUI;

        equipButton.gameObject.SetActive(canEquip);
        destroyButton.gameObject.SetActive(item != null);

        if (item == null || item.itemData == null)
        {
            Hide();
            return;
        }

        // ===== ICON & NAME =====
        itemIcon.sprite = item.itemData.icon;
        itemNameText.text = item.itemData.itemName;
        itemNameText.color = new Color(1f, 0.85f, 0.3f);

        // ===== MAIN STAT =====
        StatRoll mainRoll = item.rolls.Find(r => r.isMainStat);
        if (mainRoll != null)
        {
            mainStatText.text =
                $"{mainRoll.stat}: {mainRoll.value} <color=#888>({mainRoll.min}-{mainRoll.max})</color>";
            mainStatText.gameObject.SetActive(true);
        }
        else
        {
            mainStatText.gameObject.SetActive(false);
        }

        // ===== SUBSTATS =====
        int subIndex = 0;

        foreach (var roll in item.rolls)
        {
            if (roll.isMainStat)
                continue;

            if (subIndex >= substatTexts.Length)
                break;

            substatTexts[subIndex].text =
                $"{roll.stat}: {roll.value} <color=#888>({roll.min}-{roll.max})</color>";

            substatTexts[subIndex].gameObject.SetActive(true);
            subIndex++;
        }

        // Hide unused substat slots
        for (int i = subIndex; i < substatTexts.Length; i++)
        {
            substatTexts[i].gameObject.SetActive(false);
        }

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(OnEquipClicked);

        destroyButton.onClick.RemoveAllListeners();
        destroyButton.onClick.AddListener(OnDestroyClicked);

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }





    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

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


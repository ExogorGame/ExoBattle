using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public TextMeshProUGUI tooltipText;
    public CanvasGroup canvasGroup;
    public Button equipButton;

    private ItemInstance currentItem;
    private PlayerCharacter currentPlayer;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(ItemInstance item, PlayerCharacter player, Vector2 position, bool canEquip)
    {
        currentItem = item;
        currentPlayer = player;

        equipButton.gameObject.SetActive(canEquip); // Hide equip if item is already equipped

        tooltipText.text =
            $"<b>{item.itemData.itemName}</b>\n" +
            $"ATK: +{item.attack}\n" +
            $"DEF: +{item.defense}";

        transform.position = position;
        canvasGroup.alpha = 1;
    }


    public void Hide()
    {
        canvasGroup.alpha = 0;
        currentItem = null;
        currentPlayer = null;
    }

    public void OnEquipClicked()
    {
        if (currentItem == null || currentPlayer == null)
        {
            Debug.LogWarning("Equip failed: missing item or player");
            return;
        }

        currentPlayer.EquipItem(currentItem);
        Hide();
    }
}


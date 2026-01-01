using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel; // assign InventoryPanel
    public UnityEngine.UI.Button toggleButton;

    private bool isOpen = false;

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleInventory);
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (!isOpen)
            TooltipUI.Instance.Hide();
    }

}

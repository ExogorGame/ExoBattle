using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject inventoryPanel;
    public GameObject soulUpgradePanel;

    [Header("Buttons")]
    public Button inventoryButton;
    public Button soulsButton;

    private GameObject currentOpenPanel = null;

    void Start()
    {
        // Start panels closed
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (soulUpgradePanel != null) soulUpgradePanel.SetActive(false);

        // Wire buttons
        if (inventoryButton != null) inventoryButton.onClick.AddListener(() => TogglePanel(inventoryPanel));
        if (soulsButton != null) soulsButton.onClick.AddListener(() => TogglePanel(soulUpgradePanel));
    }

    void TogglePanel(GameObject panel)
    {
        if (panel == null) return;

        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            // Close the currently open panel
            currentOpenPanel.SetActive(false);
        }

        bool isOpening = (currentOpenPanel != panel || !panel.activeSelf);
        panel.SetActive(isOpening);

        currentOpenPanel = isOpening ? panel : null;

        // Optional: hide tooltip whenever a panel closes
        if (!isOpening)
            TooltipUI.Instance.Hide();
    }
}


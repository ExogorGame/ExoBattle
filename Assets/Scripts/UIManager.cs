using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject inventoryPanel;
    public GameObject soulUpgradePanel;
    public GameObject talentPanel;

    [Header("Main Buttons")]
    public Button inventoryButton;
    public Button soulsButton;
    public Button talentsButton;

    [Header("Zone UI")]
    public GameObject enemiesPanel;
    public ZoneUI[] zones;

    [Header("Death UI")]
    public GameObject deathPanel;
    public GameObject inputBlocker;
    public Button respawnButton;

    private GameObject currentMainPanel;
    private GameObject currentEnemyPanel;


    [System.Serializable]
    public class ZoneUI
    {
        public string zoneName;
        public Button zoneButton;
        public GameObject enemyPanel;
    }

    void Start()
    {
        // Start main panels closed
        inventoryPanel?.SetActive(false);
        soulUpgradePanel?.SetActive(false);
        enemiesPanel?.SetActive(false);
        talentPanel?.SetActive(false);


        // Wire main buttons
        inventoryButton?.onClick.AddListener(() => ToggleMainPanel(inventoryPanel));
        soulsButton?.onClick.AddListener(() => ToggleMainPanel(soulUpgradePanel));
        talentsButton?.onClick.AddListener(() => ToggleMainPanel(talentPanel));


        // Setup zones
        foreach (ZoneUI zone in zones)
        {
            zone.enemyPanel?.SetActive(false);

            if (zone.enemyPanel != null)
                WireEnemyButtons(zone.enemyPanel);

            if (zone.zoneButton != null)
            {
                ZoneUI capturedZone = zone;
                zone.zoneButton.onClick.AddListener(() => OpenZone(capturedZone));
            }
        }

    }

    // MAIN PANELS (Inventory / Souls)
    void ToggleMainPanel(GameObject panel)
    {
        if (panel == null) return;

        CloseEnemiesPanel();

        if (currentMainPanel != null && currentMainPanel != panel)
            currentMainPanel.SetActive(false);

        bool open = !panel.activeSelf;
        panel.SetActive(open);
        currentMainPanel = open ? panel : null;

        if (!open)
            TooltipUI.Instance?.Hide();
    }

    // ZONES / ENEMIES
    void OpenZone(ZoneUI zone)
    {
        if (zone.enemyPanel == null) return;

        if (enemiesPanel != null && !enemiesPanel.activeSelf)
        {
            enemiesPanel.SetActive(true);
        }

        if (currentEnemyPanel != null)
            currentEnemyPanel.SetActive(false);

        zone.enemyPanel.SetActive(true);
        currentEnemyPanel = zone.enemyPanel;
    }
    void CloseEnemiesPanel()
    {
        if (currentEnemyPanel != null)
        {
            currentEnemyPanel.SetActive(false);
            currentEnemyPanel = null;
        }

        if (enemiesPanel != null && enemiesPanel.activeSelf)
            enemiesPanel.SetActive(false);
    }

    void WireEnemyButtons(GameObject enemyPanel)
    {
        Button[] buttons = enemyPanel.GetComponentsInChildren<Button>(true);

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(CloseEnemiesPanel);
        }
    }


    private PlayerCharacter player;
    private IdleBattle battle;

    public void InitDeathUI(PlayerCharacter playerRef, IdleBattle battleRef)
    {
        player = playerRef;
        battle = battleRef;

        deathPanel?.SetActive(false);
        inputBlocker?.SetActive(false);

        if (respawnButton != null)
        {
            respawnButton.onClick.RemoveAllListeners();
            respawnButton.onClick.AddListener(OnRespawnPressed);
        }
    }

    private void OnRespawnPressed()
    {
        deathPanel?.SetActive(false);
        inputBlocker?.SetActive(false);

        player.Respawn();
    }

    public void ShowDeathUI()
    {
        inputBlocker?.SetActive(true);
        deathPanel?.SetActive(true);
    }

}




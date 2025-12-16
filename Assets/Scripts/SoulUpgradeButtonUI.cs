using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoulUpgradeButtonUI : MonoBehaviour
{
    public string soulId;

    public Image soulIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI ownedText;
    public Button upgradeButton;

    private SoulUpgradeManager mgr;
    private PlayerCharacter player;

    void Start()
    {
        mgr = SoulUpgradeManager.Instance;
        player = mgr.player;

        soulIcon.sprite =
            SoulIconDatabase.Instance.GetIcon(soulId);

        nameText.text =
            soulId.Replace("_", " ").ToUpper();

        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        int level = mgr.GetLevel(soulId);
        int cost = mgr.GetCost(soulId);
        int owned = player.GetSoulCount(soulId);

        levelText.text = $"Level {level}";
        costText.text = $"Cost: {cost}";
        ownedText.text = $"Owned: {owned}";

        upgradeButton.interactable = owned >= cost;
    }

    public void OnClickUpgrade()
    {
        mgr.Upgrade(soulId);
        Refresh();
    }
}

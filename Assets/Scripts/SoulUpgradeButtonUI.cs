using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoulUpgradeButtonUI : MonoBehaviour
{
    public string soulId;

    public Image soulIcon;
    public TextMeshProUGUI levelText;
    public Button upgradeButton;
    public TextMeshProUGUI soulsText;
    public TextMeshProUGUI descriptionText;


    private SoulUpgradeManager mgr;
    private PlayerCharacter player;

    void Start()
    {
        mgr = SoulUpgradeManager.Instance;
        player = mgr.player;

        soulIcon.sprite =
            SoulIconDatabase.Instance.GetIcon(soulId);

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

        levelText.text = $"Lv {level}";
        soulsText.text = $"{owned} / {cost}";
        soulsText.color = owned >= cost ? Color.green : Color.red;

        upgradeButton.interactable = owned >= cost;

        // Add description
        var data = mgr.upgrades.Find(u => u.soulId == soulId);
        if (data != null)
        {
            int totalBonus = data.level * data.amountPerLevel;
            descriptionText.text = $"+{totalBonus} {data.upgradeType.ToString().Replace("Power", " Power")}";

            switch (data.upgradeType)
            {
                case SoulUpgradeType.Atk: descriptionText.color = Color.red; break;
                case SoulUpgradeType.HP: descriptionText.color = Color.green; break;
                case SoulUpgradeType.Def: descriptionText.color = Color.blue; break;
            }

        }
    }


    public void OnClickUpgrade()
    {
        mgr.Upgrade(soulId);
        Refresh();
    }
}

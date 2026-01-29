using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TalentUIEntry
{
    public string talentName;
    public Button button;
    public TMP_Text label;
}

public class TalentUI : MonoBehaviour
{
    public TMP_Text pointsText;
    public TalentUIEntry[] entries;

    private PlayerCharacter player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerCharacter>();
    }

    public void Init(PlayerCharacter p)
    {
        player = p;
        Refresh();
    }

    void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        pointsText.text = $"Talent Points: {player.availableTalentPoints}";

        foreach (var e in entries)
        {
            Talent t = player.GetTalent(e.talentName);
            if (t == null) continue;

            bool unlocked = t.IsUnlocked(player);

            e.label.text = $"{t.name} ({t.currentLevel}/{t.maxLevel})";

            if (!unlocked)
            {
                e.label.text += $" (Locked: {t.requiredTalent} {t.requiredLevel})";
            }

            e.button.onClick.RemoveAllListeners();
            e.button.onClick.AddListener(() =>
            {
                player.SpendTalentPoint(t.name);
                Refresh();
            });

            e.button.interactable = player.availableTalentPoints > 0 && !t.IsMaxed && unlocked;
        }
    }

}

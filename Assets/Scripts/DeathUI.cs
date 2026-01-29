using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    public static DeathUI Instance;

    public GameObject deathPanel;
    public Button respawnButton;

    public GameObject inputBlocker;


    private PlayerCharacter player;
    private IdleBattle battle;

    void Awake()
    {
        Instance = this;
        deathPanel.SetActive(false);
    }

    public void Init(PlayerCharacter playerRef, IdleBattle battleRef)
    {
        player = playerRef;
        battle = battleRef;

        respawnButton.onClick.RemoveAllListeners();
        respawnButton.onClick.AddListener(Respawn);
    }

    public void Show()
    {
        inputBlocker.SetActive(true);
        deathPanel.SetActive(true);
    }


    void Respawn()
    {
        deathPanel.SetActive(false);
        inputBlocker.SetActive(false);

        player.Respawn();
    }
}

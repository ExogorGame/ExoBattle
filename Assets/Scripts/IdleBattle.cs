using UnityEngine;
using System.Collections;

public class IdleBattle : MonoBehaviour
{
    public PlayerCharacter player;
    public EnemyCharacter enemy;

    public InventoryUI inventoryUI;


    public float attackInterval = 2f;
    public float respawnDelay = 3f;

    private float timer = 0f;
    private bool battleActive = false;

    private void Awake()
    {
        Application.runInBackground = true;
    }
    void Start()
    {
        UIManager uiManager = Object.FindFirstObjectByType<UIManager>();
        uiManager?.InitDeathUI(player, this);

        FindFirstObjectByType<TalentUI>()?.Init(player);
    }



    void Update()
    {
        if (!battleActive) return;

        if (player.isDead)
        {
            StopBattle();
            return;
        }

        if (enemy == null) return;

        if (enemy.currentHealth <= 0)
        {
            if (!IsInvoking(nameof(RespawnEnemy)))
                Invoke(nameof(RespawnEnemy), respawnDelay);

            return;
        }

        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            player.Attack(enemy);
            enemy.Attack(player);
        }
    }

    public void StartBattle()
    {
        if (enemy == null)
        {
            Debug.LogWarning("Please select an enemy!");
            return;
        }

        if (battleActive) return;

        battleActive = true;
        timer = 0f;

        Debug.Log("Battle started!");
    }

    public void StopBattle()
    {
        battleActive = false;
        timer = 0f;

        // Cancel enemy respawn if switching enemies
        CancelInvoke(nameof(RespawnEnemy));

        Debug.Log("Battle stopped.");
    }

    void RespawnEnemy()
    {
        if (enemy == null) return;

        // XP
        player.GainLoot(enemy.xpPerEnemy);
        LootLogUI.Instance?.Show($"+{enemy.xpPerEnemy} XP");

        // Souls
        if (!string.IsNullOrEmpty(enemy.soulId))
        {
            player.AddSoul(enemy.soulId);
            LootLogUI.Instance?.Show("+1 Soul");
        }
            

        // Item drop
        ItemInstance droppedItem = enemy.TryDropItem();
        Debug.Log("TryDropItem called");
        if (droppedItem != null)
        {
            player.inventory.Add(droppedItem);
            LootLogUI.Instance?.Show(droppedItem.itemData.itemName);

            if (inventoryUI != null)
                inventoryUI.Refresh();
        }

        enemy.ResetHealth();
    }

}


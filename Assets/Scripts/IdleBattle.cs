using UnityEngine;
using System.Collections;

public class IdleBattle : MonoBehaviour
{
    public PlayerCharacter player;
    public EnemyCharacter enemy;


    public float attackInterval = 2f; // seconds
    public float respawnDelay = 3f;   // seconds before enemy respawns

    private float timer = 0f;
    private bool battleActive = false;

    void Update()
    {
        if (!battleActive) return;

        // If player is dead, stop battle
        if (player.currentHealth <= 0) return;



        // If enemy is dead, start respawn coroutine
        if (enemy.currentHealth <= 0)
        {
            if (!IsInvoking("RespawnEnemy"))
                Invoke("RespawnEnemy", respawnDelay);

            return;
        }

        // Auto attack loop
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
        Debug.Log("Battle started!");
    }


    void RespawnEnemy()

    {
        //Give Loot to Player
        player.GainLoot(enemy.xpPerEnemy, enemy.coinsDrop, enemy.bonesDrop);
        

     

        //Respawn Enemy
        enemy.ResetHealth();
        enemy.UpdateStatsUI();
        Debug.Log("Enemy has respawned!");
    }
}

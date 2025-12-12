using UnityEngine;
using System.Collections;

public class IdleBattle : MonoBehaviour
{
    public PlayerCharacter player;
    public PlayerCharacter enemy;

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
        if (battleActive) return;
        battleActive = true;
        Debug.Log("Battle started!");
    }


    void RespawnEnemy()

    {
        //Give XP to player
        player.GainXP(enemy.xpPerEnemy);

        // Award loot
        player.coins += enemy.coinsDrop; // make sure player has a coins variable
        Debug.Log($"Player received {enemy.coinsDrop} coins! Total coins: {player.coins}");
        player.bones += enemy.bonesDrop;
        Debug.Log($"Player received {enemy.bonesDrop} bones! Total bones: {player.bones} ");


        //Respawn Enemy
        enemy.ResetHealth();
        Debug.Log("Enemy has respawned!");
    }
}

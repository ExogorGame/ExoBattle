using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    public EnemyDatabase database;
    public IdleBattle battleSystem;
    public Transform enemySpawnPoint;

    private GameObject activeEnemyInstance;

    public void SelectEnemy(int index)
    {
        // STOP current battle before switching
        battleSystem.StopBattle();

        // Remove previous enemy
        if (activeEnemyInstance != null)
            Destroy(activeEnemyInstance);

        // Spawn new enemy
        EnemyCharacter enemyPrefab = database.enemies[index];
        activeEnemyInstance = Instantiate(
            enemyPrefab.gameObject,
            enemySpawnPoint,
            false
        );

        EnemyCharacter newEnemy =
            activeEnemyInstance.GetComponent<EnemyCharacter>();

        // Assign new enemy to battle system
        battleSystem.enemy = newEnemy;

        // Update UI
        Object.FindFirstObjectByType<EnemyUIManager>()
            .SetEnemy(newEnemy);

        newEnemy.UpdateStatsUI();

        Debug.Log("Selected enemy: " + newEnemy.name);
    }
}


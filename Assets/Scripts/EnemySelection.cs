using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    public EnemyDatabase database;
    public IdleBattle battleSystem;  // Assign in Inspector
    public Transform enemySpawnPoint; // Where the enemy will appear

    private GameObject activeEnemyInstance;

    public void SelectEnemy(int index)
    {
        // Remove previous enemy if one exists
        if (activeEnemyInstance != null)
            Destroy(activeEnemyInstance);

        // Spawn the new enemy
        EnemyCharacter enemyPrefab = database.enemies[index];
        activeEnemyInstance = Instantiate(enemyPrefab.gameObject, enemySpawnPoint.position, Quaternion.identity);

        // Get EnemyCharacter component
        EnemyCharacter newEnemy = activeEnemyInstance.GetComponent<EnemyCharacter>();

        // Assign it to the battle system
        battleSystem.enemy = newEnemy;

        // Tell the UI manager which enemy to show
        Object.FindFirstObjectByType<EnemyUIManager>().SetEnemy(newEnemy);

        // Update its stats UI immediately
        newEnemy.UpdateStatsUI();

        Debug.Log("Selected enemy: " + newEnemy.name);
    }
}

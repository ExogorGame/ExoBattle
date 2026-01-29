using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    public EnemyDatabase database;
    public IdleBattle battleSystem;
    public Transform enemySpawnPoint;

    private GameObject activeEnemyInstance;

    public void SelectEnemy(int index)
    {
        battleSystem.StopBattle();

        if (activeEnemyInstance != null)
            Destroy(activeEnemyInstance);

        EnemyCharacter enemyPrefab = database.enemies[index];
        activeEnemyInstance = Instantiate(
            enemyPrefab.gameObject,
            enemySpawnPoint,
            false
        );

        EnemyCharacter newEnemy = activeEnemyInstance.GetComponent<EnemyCharacter>();

        battleSystem.enemy = newEnemy;

        Object.FindFirstObjectByType<EnemyUIManager>()
            .SetEnemy(newEnemy);

        newEnemy.UpdateStatsUI();

        Debug.Log("Selected enemy: " + newEnemy.name + " (Tier " + newEnemy.itemTier + ")");
    }
}


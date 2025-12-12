using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    public EnemyDatabase database;

    public void SelectEnemy(int index)
    {
        Enemy chosen = database.enemies[index];
    }
}

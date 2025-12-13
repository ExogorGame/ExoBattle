using UnityEngine;

[CreateAssetMenu(menuName = "Database/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public EnemyCharacter[] enemies;  // These should be PREFABS
}

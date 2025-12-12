using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "IdleGame/Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int damage;
    public int reward;
    
}

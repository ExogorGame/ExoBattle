using UnityEngine;

[CreateAssetMenu(menuName = "Idle RPG/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;

    [Header("Stat Ranges")]
    public int minAttack;
    public int maxAttack;

    public int minDefense;
    public int maxDefense;

    public int minMaxHealth;
    public int maxMaxHealth;

    public int minHitRating;
    public int maxHitRating;

    public int minDodgeRating;
    public int maxDodgeRating;

    public Sprite icon;

    // Roll random stats when dropped
    public ItemInstance CreateInstance()
    {
        return new ItemInstance
        {
            itemData = this,
            attack = Random.Range(minAttack, maxAttack + 1),
            defense = Random.Range(minDefense, maxDefense + 1),
            maxHealth = Random.Range(minMaxHealth, maxMaxHealth + 1),
            hitRating = Random.Range(minHitRating, maxHitRating + 1),
            dodgeRating = Random.Range(minDodgeRating, maxDodgeRating + 1)
        };
    }
}

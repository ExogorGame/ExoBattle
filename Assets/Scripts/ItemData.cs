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

    public Sprite icon;

    // Roll random stats when dropped
    public ItemInstance CreateInstance()
    {
        return new ItemInstance
        {
            itemData = this,
            attack = Random.Range(minAttack, maxAttack + 1),
            defense = Random.Range(minDefense, maxDefense + 1)
        };
    }
}

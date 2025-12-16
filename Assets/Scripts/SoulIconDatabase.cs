using UnityEngine;
using System.Collections.Generic;

public class SoulIconDatabase : MonoBehaviour
{
    public static SoulIconDatabase Instance;

    [System.Serializable]
    public class SoulIcon
    {
        public string soulId;
        public Sprite icon;
    }

    public List<SoulIcon> icons = new();

    private Dictionary<string, Sprite> lookup =
        new Dictionary<string, Sprite>();

    private void Awake()
    {
        Instance = this;

        foreach (var s in icons)
            lookup[s.soulId] = s.icon;
    }

    public Sprite GetIcon(string soulId)
    {
        return lookup.ContainsKey(soulId) ? lookup[soulId] : null;
    }
}

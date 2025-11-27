// Assets/Data/Items/ItemDatabase.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Items/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public static ItemDatabase Instance;

    [System.Serializable]
    public class ItemData
    {
        public string id;
        public string displayName;
        public string tooltip;
        public Sprite icon;
        public string slot; // "ring", "weapon", "boots"...
        public StatModifierData[] modifiers;
    }

    public List<ItemData> items = new List<ItemData>();

    private void OnEnable() => Instance = this;

    public static ItemData GetItem(string id)
    {
        return Instance?.items.FirstOrDefault(i => i.id == id)
               ?? new ItemData { id = id, displayName = "???", tooltip = "Неизвестный предмет" };
    }
}
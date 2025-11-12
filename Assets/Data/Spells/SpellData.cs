using System;
using UnityEngine;

[Serializable]
public class SpellModifierData
{
    public StatType stat;
    public ModifierType type;
    public float value;
}

[CreateAssetMenu(menuName = "Spells/Spell Data")]
public class SpellData : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public Type passiveComponentType;

    public SpellType spellType;
    public SpellModifierData[] modifiers;
    public string[] bonuses;
    public string[] debuffs;
}
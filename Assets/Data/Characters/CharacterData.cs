using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    public string id;
    public string displayName;
    public CharacterClass characterClass;
    public Sprite portrait;
    public Color primaryColor;

    [Header("Starter Spells")]
    public string[] starterSpellIds; // ["fireball", "fire_boots", "fire_beam"]
}
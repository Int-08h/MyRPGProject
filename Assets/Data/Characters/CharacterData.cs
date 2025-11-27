using UnityEngine;

// Добавлен using System.Collections.Generic; для использования List<string> (если потребуется)

[CreateAssetMenu(menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    public string id;
    public string displayName;
    public CharacterClass characterClass;
    public Sprite portrait; // Используем для большой картинки
    public Color primaryColor;

    // ✅ НОВЫЕ ПОЛЯ ДЛЯ ОПИСАНИЯ И БОНУСОВ
    [Header("Lore & Description")]
    [TextArea(3, 10)] // Удобный редактор в Инспекторе
    public string description;
    public string[] uniqueBonuses; // Например: "Increased Spell Speed", "Mana on Kill"

    [Header("Starter Spells")]
    public string[] starterSpellIds; // ["fireball", "fire_boots", "fire_beam"]
}
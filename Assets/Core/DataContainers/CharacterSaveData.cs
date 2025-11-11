using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    // Идентификация
    public string id;             // "fire_01", "ice_01"...
    public string displayName;

    // Прогресс
    public int level = 1;
    public int experience = 0;
    public int unspentStatPoints = 5;
    public int unspentSkillPoints = 0;

    // Конфигурация
    public string selectedStarterSpell = ""; // "fireball", "frost_nova"...

    // Предметы
    public Dictionary<string, string> equippedItems = new Dictionary<string, string>();
    // ключ: slotName ("head", "weapon", "ring1"...)
    // значение: itemID ("ring_of_focus")

    // Навыки и благословения
    public List<string> unlockedSkillIds = new List<string>();
    public List<string> blessings = new List<string>();

    // Пройденные карты
    public List<string> completedMapIds = new List<string>();

    // Статы вручную НЕ хранятся (вычисляются через StatsCalculator)

    public static CharacterSaveData CreateNew(string charId)
    {
        // Используем базу персонажей → пока заглушка:
        var data = new CharacterSaveData { id = charId };
        if (charId.StartsWith("fire")) data.displayName = "Кайрос";
        else if (charId.StartsWith("ice")) data.displayName = "Элира";
        else if (charId.StartsWith("earth")) data.displayName = "Тера";
        else if (charId.StartsWith("air")) data.displayName = "Зефир";
        else if (charId.StartsWith("lightning")) data.displayName = "Вольт";

        return data;
    }
}
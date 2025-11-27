// CharacterSaveData.txt
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    // --- Идентификация ---
    public string id;             // "fire_01", "ice_01"...
    public string displayName;

    // --- Прогресс ---
    public int level = 1;
    public int experience = 0;
    public int unspentStatPoints = 5;
    public int unspentSkillPoints = 0;

    // --- Конфигурация ---
    public string selectedStarterSpell = ""; // "fireball", "frost_nova"...

    // --- Предметы ---
    public Dictionary<string, string> equippedItems = new Dictionary<string, string>();
    // ✅ ИСПРАВЛЕНО: НОВОЕ ПОЛЕ: Список неэкипированных предметов в инвентаре
    public List<string> inventoryItemIds = new List<string>();

    // --- Навыки и благословения ---
    public List<string> unlockedSkillIds = new List<string>();
    public List<string> blessings = new List<string>();

    // --- Пройденные карты ---
    public List<string> completedMapIds = new List<string>();

    // ----------------------------------------------------------------------

    public static CharacterSaveData CreateNew(string charId)
    {
        var data = new CharacterSaveData { id = charId };

        // ✅ ИСПРАВЛЕНО: Используем CharacterDatabase для получения имени.
        var charData = CharacterDatabase.GetCharacterData(charId);

        if (charData != null)
        {
            // 1. Берем отображаемое имя из ассета (Убираем жесткое кодирование)
            data.displayName = charData.displayName;

            // 2. Устанавливаем первое стартовое заклинание по умолчанию (если оно есть)
            if (charData.starterSpellIds != null && charData.starterSpellIds.Length > 0)
            {
                data.selectedStarterSpell = charData.starterSpellIds[0];
            }
        }
        else
        {
            data.displayName = "Неизвестный герой";
            Debug.LogError($"[CharacterSaveData] CharacterData для ID '{charId}' не найдена в CharacterDatabase.");
        }

        // data.inventoryItemIds.Add("starting_sword"); 

        return data;
    }
}
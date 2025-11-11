using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private static string GlobalSavePath => Path.Combine(Application.persistentDataPath, "global_save.json");
    private static string CharacterSavePath(string id) => Path.Combine(Application.persistentDataPath, $"character_{id}.json");

    // óóó √ÀŒ¡¿À‹ÕŒ≈ —Œ’–¿Õ≈Õ»≈ óóó
    public static void SaveGlobalState()
    {
        var data = new GlobalSaveData
        {
            selectedMode = GameManager.selectedMode,
            selectedMap = GameManager.selectedMap,
            selectedDifficulty = GameManager.selectedDifficulty,
            selectedCharacterId = GameManager.selectedCharacterId
        };
        File.WriteAllText(GlobalSavePath, JsonUtility.ToJson(data));
    }

    public static GlobalSaveData LoadGlobalState()
    {
        if (!File.Exists(GlobalSavePath)) return new GlobalSaveData();
        return JsonUtility.FromJson<GlobalSaveData>(File.ReadAllText(GlobalSavePath));
    }

    // óóó œ≈–—ŒÕ¿∆ óóó
    public static void SaveCharacter(CharacterSaveData c)
    {
        Debug.Log($"[SaveSystem] Saving character {c.id}");
        File.WriteAllText(CharacterSavePath(c.id), JsonUtility.ToJson(c, true));
    }

    public static CharacterSaveData LoadCharacter(string id)
    {
        string path = CharacterSavePath(id);
        if (!File.Exists(path)) return null;
        try
        {
            return JsonUtility.FromJson<CharacterSaveData>(File.ReadAllText(path));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] Corrupted save for {id}: {e}");
            return null;
        }
    }

    // óóó œ–Œ¬≈– ¿ œ–Œ√–≈——¿ óóó
    public static bool HasMeaningfulProgress()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "character_*.json");
        if (files.Length == 0) return false;

        foreach (string path in files)
        {
            try
            {
                var data = JsonUtility.FromJson<CharacterSaveData>(File.ReadAllText(path));
                if (data == null) continue;
                if (data.level > 1 ||
                    data.experience > 0 ||
                    data.unspentStatPoints < 5 ||
                    data.unlockedSkillIds.Count > 0 ||
                    data.equippedItems.Count > 0 ||
                    data.completedMapIds.Count > 0 ||
                    data.blessings.Count > 0)
                    return true;
            }
            catch { /* corrupted ó ignore */ }
        }
        return false;
    }
}
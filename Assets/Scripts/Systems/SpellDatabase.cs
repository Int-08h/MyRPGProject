// Scripts/Systems/SpellDatabase.cs
using System.Collections.Generic;
using UnityEngine;

public static class SpellDatabase
{
    private static Dictionary<string, SpellData> _cache;
    private const string SPELLS_PATH = "Spells/";

    public static SpellData GetSpell(string id)
    {
        if (_cache == null)
            LoadAllSpells();

        return _cache != null && _cache.TryGetValue(id, out var spell) ? spell : null;
    }

    private static void LoadAllSpells()
    {
        _cache = new Dictionary<string, SpellData>();
        var spells = Resources.LoadAll<SpellData>(SPELLS_PATH);
        foreach (var spell in spells)
        {
            if (!string.IsNullOrEmpty(spell.id))
                _cache[spell.id] = spell;
        }
        Debug.Log($"SpellDatabase loaded {_cache.Count} spells.");
    }

    // Опционально: перезагрузка (для редактора / отладки)
    public static void Reload() => _cache = null;
}
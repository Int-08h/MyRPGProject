// Assets/Data/Spells/SpellDatabase.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Spells/Spell Database")]
public class SpellDatabase : ScriptableObject
{
    public static SpellDatabase Instance;

    public List<SpellData> spells = new List<SpellData>();

    private void OnEnable()
    {
        // Присваиваем Instance, когда ScriptableObject включен
        Instance = this;
    }

    /// <summary>
    /// Получает данные заклинания по его ID (Теперь СТАТИЧЕСКИЙ).
    /// </summary>
    public static SpellData GetSpell(string id) // <-- ДОБАВЛЕНО static
    {
        // Используем статический Instance для доступа к списку
        return Instance?.spells.FirstOrDefault(s => s.id == id);
    }

}
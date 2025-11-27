// Assets/Data/Characters/CharacterDatabase.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Characters/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    // Статический Instance позволяет легко обращаться к базе из любого места
    public static CharacterDatabase Instance;

    // Здесь хранятся все ассеты CharacterData
    public List<CharacterData> characters = new List<CharacterData>();

    // Присваиваем Instance, когда ScriptableObject загружен
    private void OnEnable() => Instance = this;

    /// <summary>
    /// Получает данные персонажа по его ID.
    /// </summary>
    public static CharacterData GetCharacterData(string id)
    {
        return Instance?.characters.FirstOrDefault(c => c.id == id);
    }
}
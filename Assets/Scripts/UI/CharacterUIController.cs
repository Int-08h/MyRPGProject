// Assets/Scripts/UI/CharacterUIController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CharacterUIController : MonoBehaviour
{
    [Header("UI References")]
    public Image characterPortraitImage;
    public TMP_Text characterDescriptionText;
    public Transform magicGrid; // Сетка для кнопок заклинаний
    public TMP_Text magicDescriptionText;

    [Header("Prefabs")]
    public GameObject magicButtonPrefab;

    // ✅ Ссылка на ГЛАВНЫЙ КОНТРОЛЛЕР
    private CharacterSelectController parentController;

    // Внутренний список для управления подсветкой кнопок заклинаний
    private List<MagicButton> magicButtons = new List<MagicButton>();

    public void Initialize(CharacterSelectController controller)
    {
        parentController = controller;
    }

    // ----------------------------------------------------
    // МЕТОДЫ ОБНОВЛЕНИЯ UI
    // ----------------------------------------------------

    /// <summary>
    /// Обновляет портрет, описание персонажа и список стартовых заклинаний.
    /// </summary>
    public void RefreshCharacterAndMagicUI()
    {
        CharacterSaveData currentCharacter = parentController.CurrentCharacter;
        if (currentCharacter == null) return;

        // Получаем ассет данных персонажа
        CharacterData charData = CharacterDatabase.GetCharacterData(currentCharacter.id);
        if (charData == null) return;

        // 1. Обновление описания персонажа
        characterDescriptionText.text = $"<b>{charData.displayName}</b> ({charData.characterClass})\n\n{charData.description}\n\n";
        characterDescriptionText.text += "--- Бонусы ---\n";
        if (charData.uniqueBonuses != null)
        {
            characterDescriptionText.text += string.Join("\n", charData.uniqueBonuses.Select(b => $"• {b}"));
        }

        // 2. Обновление портрета
        if (charData.portrait)
        {
            characterPortraitImage.sprite = charData.portrait;
        }

        // 3. Обновление UI заклинаний (создание кнопок)
        RefreshMagicButtons(charData);

        // 4. Обновление описания выбранного заклинания
        RefreshMagicDescription();
    }

    /// <summary>
    /// Создает или обновляет кнопки стартовых заклинаний.
    /// </summary>
    private void RefreshMagicButtons(CharacterData charData)
    {
        // 1. Очистка старых кнопок
        foreach (var btn in magicButtons)
        {
            Destroy(btn.gameObject);
        }
        magicButtons.Clear();

        // 2. Создание новых кнопок
        foreach (string spellId in charData.starterSpellIds)
        {
            // Используем статический вызов SpellDatabase.GetSpell() (Как мы исправили в прошлый раз)
            var spellData = SpellDatabase.GetSpell(spellId);

            if (spellData != null)
            {
                var btnObject = Instantiate(magicButtonPrefab, magicGrid);
                var btnScript = btnObject.GetComponent<MagicButton>();

                // Инициализируем кнопку, передавая метод контроллера для обработки клика
                btnScript.Initialize(spellData, parentController.OnSpellSelected);

                // Проверяем, является ли это заклинание выбранным по умолчанию
                if (parentController.CurrentCharacter.selectedStarterSpell == spellId)
                {
                    btnScript.Select();
                }
                else
                {
                    btnScript.Deselect();
                }

                magicButtons.Add(btnScript);
            }
        }
    }

    /// <summary>
    /// Обновляет описание выбранного заклинания.
    /// </summary>
    public void RefreshMagicDescription()
    {
        if (parentController.CurrentCharacter == null) return;

        string selectedId = parentController.CurrentCharacter.selectedStarterSpell;

        // Используем статический вызов SpellDatabase.GetSpell()
        var spellData = SpellDatabase.GetSpell(selectedId);

        if (spellData != null)
        {
            string description = $"<b>{spellData.displayName}</b>: {spellData.description}\n\n";

            // Добавление бонусов
            if (spellData.bonuses != null && spellData.bonuses.Length > 0)
            {
                description += "<color=#4A90E2>Бонусы:</color>\n";
                description += string.Join("\n", spellData.bonuses.Select(b => $"• {b}"));
            }

            magicDescriptionText.text = description;
        }
        else
        {
            magicDescriptionText.text = "Заклинание не выбрано или не найдено.";
        }
    }

    /// <summary>
    /// Обновляет подсветку кнопок заклинаний (вызывается из CharacterSelectController.OnSpellSelected).
    /// </summary>
    public void UpdateMagicButtonHighlight(string selectedSpellId)
    {
        foreach (var btn in magicButtons)
        {
            if (btn.spellId == selectedSpellId)
            {
                btn.Select();
            }
            else
            {
                btn.Deselect();
            }
        }
        RefreshMagicDescription();
    }
}
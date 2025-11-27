// Assets/Scripts/UI/CharacterSelectController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CharacterSelectController : MonoBehaviour
{
    // --- НОВЫЕ ПОЛЯ ДЛЯ ДОЧЕРНИХ КОНТРОЛЛЕРОВ ---
    [Header("Sub-Controllers (Facades)")]
    public CharacterUIController characterUIController;
    public InventoryController inventoryController;
    // public StatsController statsController; // Будут добавлены позже
    // public SkillsController skillsController; // Будут добавлены позже

    // --- ССЫЛКИ НА UI ЭЛЕМЕНТЫ (только для глобального управления) ---
    [Header("Tabs")]
    public GameObject tab_Select;
    public GameObject tab_Inventory;
    public GameObject tab_Stats;
    public GameObject tab_Skills;

    [Header("UI: Character Select")]
    public Transform characterButtonGrid;

    [Header("UI: Buttons")]
    public Button backButton;
    public Button startButton;

    [Header("Data")]
    // Data-классы можно убрать, так как они доступны статически,
    // но оставляем, чтобы можно было перетащить их в Инспекторе и убедиться, что они загружены.
    public CharacterDatabase characterDatabase;
    public SpellDatabase spellDatabase;
    public ItemDatabase itemDatabase;

    [Header("Prefabs")]
    public GameObject characterButtonPrefab;
    public GameObject magicButtonPrefab; // Теперь используется в CharacterUIController

    // --- ВНУТРЕННЕЕ СОСТОЯНИЕ ---
    private CharacterSaveData currentCharacter;
    public CharacterSaveData CurrentCharacter => currentCharacter;

    private CharacterButton selectedCharacterButton;

    // ----------------------------------------------------
    // STARTUP & INITIALIZATION
    // ----------------------------------------------------

    void Start()
    {
        // 1. Инициализация дочерних контроллеров
        characterUIController.Initialize(this);
        inventoryController.Initialize(this);

        // 2. Загрузка данных из GameManager
        string charId = GameManager.selectedCharacterId;
        LoadCharacterSelection(charId);

        // 3. Создание кнопок персонажей
        CreateCharacterButtons();

        // 4. Установка первой вкладки по умолчанию
        SwitchTab(tab_Select);
    }

    /// <summary>
    /// Создает кнопки для всех доступных персонажей.
    /// </summary>
    void CreateCharacterButtons()
    {
        // Используем CharacterDatabase.Instance, если не привязан публичный CharacterDatabase
        var charDatas = characterDatabase?.characters ?? CharacterDatabase.Instance?.characters;

        if (charDatas == null) return;

        foreach (var data in charDatas)
        {
            var btnObject = Instantiate(characterButtonPrefab, characterButtonGrid);
            var btnScript = btnObject.GetComponent<CharacterButton>();

            // Инициализируем кнопку, передавая метод для обработки клика
            btnScript.Initialize(data, OnSelectCharacter);

            // Если этот персонаж был выбран ранее, подсвечиваем его
            if (currentCharacter != null && data.id == currentCharacter.id)
            {
                selectedCharacterButton = btnScript;
                selectedCharacterButton.Select();
            }
        }
    }

    /// <summary>
    /// Загружает данные сохранения или создает новые для выбранного персонажа.
    /// </summary>
    void LoadCharacterSelection(string charId)
    {
        if (string.IsNullOrEmpty(charId))
        {
            // Если ID не установлен, берем первого персонажа
            charId = characterDatabase?.characters?.FirstOrDefault()?.id;
            if (string.IsNullOrEmpty(charId))
            {
                Debug.LogError("Нет доступных персонажей для выбора!");
                return;
            }
        }

        // 1. Пытаемся загрузить существующий прогресс
        currentCharacter = SaveSystem.LoadCharacter(charId);

        if (currentCharacter == null)
        {
            // 2. Если сохранения нет, создаем новое
            currentCharacter = CharacterSaveData.CreateNew(charId);
            // Если это новый персонаж, дадим ему несколько стартовых предметов для теста
            if (currentCharacter.inventoryItemIds.Count == 0)
            {
                currentCharacter.inventoryItemIds.AddRange(new[] { "bronze_sword", "iron_helmet", "ring_of_focus", "health_potion" });
            }
            // Сохраняем новое состояние
            SaveSystem.SaveCharacter(currentCharacter);
        }

        GameManager.selectedCharacterId = currentCharacter.id;
        RefreshAllUI();
    }

    // ----------------------------------------------------
    // ОСНОВНЫЕ МЕТОДЫ ОБНОВЛЕНИЯ
    // ----------------------------------------------------

    /// <summary>
    /// Обновляет все UI-элементы.
    /// </summary>
    public void RefreshAllUI()
    {
        characterUIController.RefreshCharacterAndMagicUI();
        inventoryController.RefreshInventoryUI();
        inventoryController.RefreshEquippedSlotsUI();
        // statsController.RefreshStatsUI(); 
        // skillsController.RefreshSkillsUI();
    }

    // ----------------------------------------------------
    // ОБРАБОТЧИКИ UI-СОБЫТИЙ
    // ----------------------------------------------------

    /// <summary>
    /// Вызывается при нажатии на кнопку персонажа.
    /// </summary>
    public void OnSelectCharacter(string charId)
    {
        if (currentCharacter != null && currentCharacter.id == charId) return;

        // 1. Сохраняем прогресс текущего персонажа перед сменой
        if (currentCharacter != null)
        {
            SaveSystem.SaveCharacter(currentCharacter);
        }

        // 2. Снимаем подсветку со старой кнопки
        selectedCharacterButton?.Deselect();

        // 3. Загружаем нового персонажа
        LoadCharacterSelection(charId);

        // 4. Подсвечиваем новую кнопку
        var newSelectedButton = characterButtonGrid.GetComponentsInChildren<CharacterButton>().FirstOrDefault(b => b.charId == charId);
        if (newSelectedButton != null)
        {
            selectedCharacterButton = newSelectedButton;
            selectedCharacterButton.Select();
        }
    }

    /// <summary>
    /// Вызывается при нажатии на кнопку заклинания.
    /// </summary>
    public void OnSpellSelected(string spellId)
    {
        if (currentCharacter.selectedStarterSpell == spellId) return;

        currentCharacter.selectedStarterSpell = spellId;
        SaveSystem.SaveCharacter(currentCharacter);

        // 💡 Делегируем обновление UI дочернему контроллеру
        characterUIController.UpdateMagicButtonHighlight(spellId);
    }

    /// <summary>
    /// Экипирует или снимает предмет (вызывается из InventorySlot).
    /// </summary>
    public void EquipItem(string slotName, string itemID)
    {
        // 💡 Логика переноса предмета (удаление/добавление в equippedItems) 
        // остается здесь, чтобы иметь централизованный доступ к CharacterSaveData.

        if (string.IsNullOrEmpty(itemID))
        {
            // Снять предмет
            currentCharacter.equippedItems.Remove(slotName);
        }
        else
        {
            // Экипировать
            currentCharacter.equippedItems[slotName] = itemID;
        }

        SaveSystem.SaveCharacter(currentCharacter);
        // Обратите внимание: InventorySlot вызывает RefreshAllUI после EquipItem(),
        // чтобы обновить инвентарь и слоты экипировки.
    }

    /// <summary>
    /// Переключает активную вкладку.
    /// </summary>
    public void SwitchTab(GameObject targetTab)
    {
        tab_Select.SetActive(false);
        tab_Inventory.SetActive(false);
        tab_Stats.SetActive(false);
        tab_Skills.SetActive(false);

        targetTab.SetActive(true);
    }

    public void OnBack()
    {
        SaveSystem.SaveGlobalState();
        SceneManager.LoadScene("MapSelect");
    }

    public void OnStartGame()
    {
        if (currentCharacter != null && !string.IsNullOrEmpty(GameManager.selectedMap) && !string.IsNullOrEmpty(currentCharacter.selectedStarterSpell))
        {
            // 1. Сохраняем финальные данные персонажа
            SaveSystem.SaveCharacter(currentCharacter);

            // 2. Устанавливаем выбранного персонажа в GameManager (уже сделано в LoadCharacterSelection, но для надежности)
            GameManager.selectedCharacterId = currentCharacter.id;

            // 3. Переход на сцену игры
            SceneManager.LoadScene("GameScene"); // Или любая другая сцена игры
        }
        else
        {
            Debug.LogWarning("Невозможно начать игру: Карта или стартовое заклинание не выбраны!");
        }
    }
}
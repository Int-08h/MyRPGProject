//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections.Generic;

//public class CharacterSelectController : MonoBehaviour
//{
//    // ——— ВКЛАДКИ ———
//    public GameObject tab_Select, tab_Inventory, tab_Stats, tab_Skills;
//    public Button[] tabButtons;

//    // ——— ВЫБОР ———
//    public Transform characterList;
//    public Transform magicGrid;
//    public TMP_Text magicDescriptionText;
//    public Outline selectedSpellOutline;

//    // ——— КНОПКИ ———
//    public Button backButton;
//    public Button startButton;

//    // ——— ДАННЫЕ ———
//    private CharacterSaveData currentCharacter;
//    private CharacterData selectedCharacterData;

//    private GameObject[] characterButtons;
//    private MagicButton[] magicButtons;

//    void Start()
//    {
//        SetupTabs();
//        SetupButtons();

//        // Загрузка персонажа (из GameManager)
//        string charId = GameManager.selectedCharacterId;
//        if (string.IsNullOrEmpty(charId))
//        {
//            // Берём первого по умолчанию (fire_01)
//            charId = "fire_01";
//        }

//        LoadCharacter(charId);
//        RefreshUI();
//    }

//    void SetupTabs()
//    {
//        tabButtons[0].onClick.AddListener(() => SwitchTab(tab_Select));
//        tabButtons[1].onClick.AddListener(() => SwitchTab(tab_Inventory));
//        tabButtons[2].onClick.AddListener(() => SwitchTab(tab_Stats));
//        tabButtons[3].onClick.AddListener(() => SwitchTab(tab_Skills));

//        SwitchTab(tab_Select); // по умолчанию
//    }

//    void SetupButtons()
//    {
//        backButton.onClick.AddListener(OnBack);
//        startButton.onClick.AddListener(OnStart);
//    }

//    // ——— ЗАГРУЗКА ПЕРСОНАЖА ———
//    void LoadCharacter(string charId)
//    {
//        currentCharacter = SaveSystem.LoadCharacter(charId)
//                        ?? CharacterSaveData.CreateNew(charId);

//        // Получаем CharacterData по id
//        selectedCharacterData = Resources.Load<CharacterData>($"Characters/{charId}");
//        if (selectedCharacterData == null)
//        {
//            Debug.LogError($"CharacterData for {charId} not found!");
//            // fallback — создадим временный
//            selectedCharacterData = ScriptableObject.CreateInstance<CharacterData>();
//            selectedCharacterData.id = charId;
//            selectedCharacterData.starterSpellIds = new[] { "fireball", "fire_boots", "fire_beam" };
//        }

//        // Если магия ещё не выбрана — выбираем первую
//        if (string.IsNullOrEmpty(currentCharacter.selectedStarterSpell) && selectedCharacterData.starterSpellIds.Length > 0)
//        {
//            currentCharacter.selectedStarterSpell = selectedCharacterData.starterSpellIds[0];
//            SaveSystem.SaveCharacter(currentCharacter); // ✅ Автосохранение!
//        }

//        RefreshCharacterList();
//        RefreshMagicGrid();
//    }

//    // ——— ОБНОВЛЕНИЕ UI ———
//    void RefreshCharacterList()
//    {
//        // Очистка
//        foreach (Transform t in characterList) Destroy(t.gameObject);

//        // Получаем все персонажи (заглушка: 5 шт.)
//        string[] charIds = { "fire_01", "ice_01", "earth_01", "air_01", "lightning_01" };
//        characterButtons = new GameObject[charIds.Length];

//        for (int i = 0; i < charIds.Length; i++)
//        {
//            var btn = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CharacterButton"), characterList);
//            var comp = btn.GetComponent<CharacterButton>();
//            comp.Setup(charIds[i], this);
//            if (charIds[i] == currentCharacter.id)
//                comp.Select();
//            characterButtons[i] = btn;
//        }
//    }

//    void RefreshMagicGrid()
//    {
//        // Очистка
//        foreach (Transform t in magicGrid) Destroy(t.gameObject);

//        magicButtons = new MagicButton[selectedCharacterData.starterSpellIds.Length];

//        for (int i = 0; i < selectedCharacterData.starterSpellIds.Length; i++)
//        {
//            var btn = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MagicButton"), magicGrid);
//            var comp = btn.GetComponent<MagicButton>();
//            string spellId = selectedCharacterData.starterSpellIds[i];
//            comp.Setup(spellId, this);
//            if (spellId == currentCharacter.selectedStarterSpell)
//                comp.Select();
//            magicButtons[i] = btn;
//        }
//    }

//    void RefreshMagicDescription()
//    {
//        var spell = SpellDatabase.Get(currentCharacter.selectedStarterSpell);
//        if (spell == null)
//        {
//            magicDescriptionText.text = "Неизвестное заклинание";
//            return;
//        }

//        string desc = $"<b>{spell.displayName}</b>\n{spell.description}\n\n";
//        if (spell.bonuses.Length > 0)
//        {
//            desc += "<color=#4A90E2>Бонусы:</color>\n";
//            foreach (var b in spell.bonuses) desc += $"• {b}\n";
//        }
//        if (spell.debuffs.Length > 0)
//        {
//            desc += "<color=#D00000>Дебафы:</color>\n";
//            foreach (var d in spell.debuffs) desc += $"• {d}\n";
//        }
//        magicDescriptionText.text = desc;
//    }

//    // ——— УПРАВЛЕНИЕ ———
//    public void OnCharacterSelected(string charId)
//    {
//        if (currentCharacter.id == charId) return;

//        currentCharacter = SaveSystem.LoadCharacter(charId)
//                        ?? CharacterSaveData.CreateNew(charId);
//        LoadCharacter(charId);
//        RefreshUI();
//    }

//    public void OnSpellSelected(string spellId)
//    {
//        if (currentCharacter.selectedStarterSpell == spellId) return;

//        currentCharacter.selectedStarterSpell = spellId;
//        SaveSystem.SaveCharacter(currentCharacter); // ✅ Автосохранение!
//        RefreshMagicDescription();

//        // Обновляем UI: снимаем выделение со всех → выделяем текущую
//        foreach (var btn in magicButtons) btn.Deselect();
//        var selectedBtn = System.Array.Find(magicButtons, b => b.spellId == spellId);
//        selectedBtn?.Select();
//    }

//    void RefreshUI()
//    {
//        RefreshMagicDescription();
//    }

//    void SwitchTab(GameObject targetTab)
//    {
//        tab_Select.SetActive(targetTab == tab_Select);
//        tab_Inventory.SetActive(targetTab == tab_Inventory);
//        tab_Stats.SetActive(targetTab == tab_Stats);
//        tab_Skills.SetActive(targetTab == tab_Skills);
//    }

//    public void OnBack()
//    {
//        SaveSystem.SaveGlobalState(); // сохраняем выбор персонажа
//        SceneManager.LoadScene("MapSelect");
//    }

//    public void OnStart()
//    {
//        // Финальное сохранение
//        SaveSystem.SaveCharacter(currentCharacter);
//        SaveSystem.SaveGlobalState();

//        SceneManager.LoadScene("GameScene"); // или "TestScene"
//    }
//}
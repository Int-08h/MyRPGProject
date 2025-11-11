using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapSelectController : MonoBehaviour
{
    [Header("Maps")]
    public Button[] mapButtons; // 6 кнопок
    public string[] mapIds = { "ashen_wastes", "icy_crypt", "volcanic_peaks", "forest_of_whispers", "desert_of_echoes", "ruins_of_dawn" };

    [Header("Difficulty")]
    public Toggle[] difficultyToggles; // 5 toggle'ов
    public Difficulty[] difficultyValues = {
        Difficulty.Scout,
        Difficulty.Vanguard,
        Difficulty.Champion,
        Difficulty.Master,
        Difficulty.Nameless
    };

    [Header("Preview")]
    public Image previewImage;
    public TMP_Text descriptionText;
    public Image affinityIcon;

    [Header("Buttons")]
    public Button backButton;
    public Button confirmButton;

    private string currentSelectedMapId = "";
    private Difficulty currentSelectedDifficulty = Difficulty.None;

    void Start()
    {
        // Подключаем события
        for (int i = 0; i < mapButtons.Length; i++)
        {
            int index = i;
            mapButtons[i].onClick.AddListener(() => OnMapSelected(mapIds[index]));
        }

        for (int i = 0; i < difficultyToggles.Length; i++)
        {
            int index = i;
            difficultyToggles[i].onValueChanged.AddListener((value) =>
            {
                if (value)
                    OnDifficultySelected(difficultyValues[index]);
            });
        }

        backButton.onClick.AddListener(OnBack);
        confirmButton.onClick.AddListener(OnConfirm);

        // Загружаем ранее выбранные значения
        LoadPreviousSelection();
    }

    void LoadPreviousSelection()
    {
        // Если есть сохранённая карта — подсвечиваем её
        if (!string.IsNullOrEmpty(GameManager.selectedMap))
        {
            currentSelectedMapId = GameManager.selectedMap;
            HighlightSelectedMap(currentSelectedMapId);
            UpdatePreview(currentSelectedMapId);
        }

        // Если есть сохранённая сложность — активируем toggle
        if (GameManager.selectedDifficulty != Difficulty.None)
        {
            currentSelectedDifficulty = GameManager.selectedDifficulty;
            HighlightSelectedDifficulty(currentSelectedDifficulty);
        }
    }

    void HighlightSelectedMap(string mapId)
    {
        for (int i = 0; i < mapButtons.Length; i++)
        {
            Color color = mapIds[i] == mapId ? new Color(0.4f, 0.9f, 0.6f) : new Color(0.7f, 0.7f, 0.7f);
            mapButtons[i].GetComponent<Image>().color = color;
        }
    }

    void HighlightSelectedDifficulty(Difficulty diff)
    {
        for (int i = 0; i < difficultyToggles.Length; i++)
        {
            difficultyToggles[i].isOn = difficultyValues[i] == diff;
        }
    }

    public void OnMapSelected(string mapId)
    {
        currentSelectedMapId = mapId;
        GameManager.selectedMap = mapId;
        UpdatePreview(mapId);
        HighlightSelectedMap(mapId);
    }

    public void OnDifficultySelected(Difficulty diff)
    {
        currentSelectedDifficulty = diff;
        GameManager.selectedDifficulty = diff;
    }

    void UpdatePreview(string mapId)
    {
        // Здесь можно загрузить изображение, описание и иконку из базы данных
        // Пока заглушка:
        previewImage.color = Color.gray; // Заглушка — позже заменим на реальные спрайты
        descriptionText.text = $"Описание карты: {mapId}";
        affinityIcon.color = Color.white; // Заглушка
    }

    public void OnBack()
    {
        GameManager.selectedMap = null;
        GameManager.selectedDifficulty = Difficulty.None;
        SceneManager.LoadScene("ModeSelect");
    }

    public void OnConfirm()
    {
        if (string.IsNullOrEmpty(currentSelectedMapId))
        {
            Debug.LogWarning("Карта не выбрана!");
            return;
        }

        if (currentSelectedDifficulty == Difficulty.None)
        {
            Debug.LogWarning("Сложность не выбрана!");
            return;
        }

        SceneManager.LoadScene("CharacterSelect");
    }
}
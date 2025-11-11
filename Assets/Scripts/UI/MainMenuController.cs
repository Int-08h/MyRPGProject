using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button NewGameButton;
    public Button ContinueButton;
    public Button SettingsButton;
    public Button QuitButton;

    [Header("Panels")]
    public GameObject SettingsPanel;

    void Start()
    {
        // Активность кнопки "Продолжить" зависит от наличия прогресса
        ContinueButton.gameObject.SetActive(SaveSystem.HasMeaningfulProgress());

        // Подключаем события
        NewGameButton.onClick.AddListener(OnNewGame);
        ContinueButton.onClick.AddListener(OnContinue);
        SettingsButton.onClick.AddListener(OnSettings);
        QuitButton.onClick.AddListener(OnQuit);

        if (SettingsPanel != null)
        {
            var backBtn = SettingsPanel.GetComponentInChildren<Button>();
            if (backBtn != null)
                backBtn.onClick.AddListener(() => SettingsPanel.SetActive(false));
        }
    }

    public void OnNewGame()
    {
        GameManager.Reset();
        SceneManager.LoadScene("ModeSelect");
    }

    public void OnContinue()
    {
        var global = SaveSystem.LoadGlobalState();
        GameManager.selectedMode = global.selectedMode;
        GameManager.selectedMap = global.selectedMap;
        GameManager.selectedDifficulty = global.selectedDifficulty;
        GameManager.selectedCharacterId = global.selectedCharacterId;

        SceneManager.LoadScene("ModeSelect");
    }

    public void OnSettings()
    {
        SettingsPanel.SetActive(true);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
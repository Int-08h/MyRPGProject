using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelectController : MonoBehaviour
{
    [Header("Buttons")]
    public Button CampaignButton;
    public Button SurvivalButton;
    public Button SandboxButton;
    public Button BackButton;

    [Header("Visuals")]
    public Color selectedColor = new Color(0.4f, 0.9f, 0.6f); // Зеленоватый
    public Color defaultColor = new Color(0.7f, 0.7f, 0.7f); // Серый

    void Start()
    {
        // Подключаем события
        CampaignButton.onClick.AddListener(() => OnModeSelected(GameMode.Campaign));
        SurvivalButton.onClick.AddListener(() => OnModeSelected(GameMode.Survival));
        SandboxButton.onClick.AddListener(() => OnModeSelected(GameMode.Sandbox));
        BackButton.onClick.AddListener(OnBack);

        // Подсветка ранее выбранного режима
        HighlightSelectedMode();
    }

    void HighlightSelectedMode()
    {
        // Сбрасываем все цвета
        SetButtonColor(CampaignButton, defaultColor);
        SetButtonColor(SurvivalButton, defaultColor);
        SetButtonColor(SandboxButton, defaultColor);

        // Подсвечиваем текущий
        switch (GameManager.selectedMode)
        {
            case GameMode.Campaign:
                SetButtonColor(CampaignButton, selectedColor);
                break;
            case GameMode.Survival:
                SetButtonColor(SurvivalButton, selectedColor);
                break;
            case GameMode.Sandbox:
                SetButtonColor(SandboxButton, selectedColor);
                break;
        }
    }

    void SetButtonColor(Button button, Color color)
    {
        if (button != null && button.GetComponent<Image>() != null)
            button.GetComponent<Image>().color = color;
    }

    public void OnModeSelected(GameMode mode)
    {
        GameManager.selectedMode = mode;
        SceneManager.LoadScene("MapSelect");
    }

    public void OnBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
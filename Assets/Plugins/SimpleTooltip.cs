// Assets/Plugins/SimpleTooltip.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class SimpleTooltip : MonoBehaviour
{
    public static SimpleTooltip Instance;

    public TMP_Text textComponent;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) gameObject.AddComponent<CanvasGroup>();
        Hide();
    }

    public static void Show(string content)
    {
        if (Instance == null || string.IsNullOrEmpty(content)) return;

        Instance.textComponent.text = content;
        Instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (Instance == null) return;
        Instance.gameObject.SetActive(false);
    }

    private void LateUpdate()  // ← LateUpdate, чтобы перекрыть UI-анимации
    {
        if (!gameObject.activeSelf) return;

        // ✅ Исправление: приводим оба к Vector2
        Vector2 pos = (Vector2)Input.mousePosition + Vector2.up * 20f;
        ((RectTransform)transform).position = pos;
    }
}
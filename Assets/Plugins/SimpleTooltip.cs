//using UnityEngine;
//using TMPro;

//public class SimpleTooltip : MonoBehaviour
//{
//    public static SimpleTooltip Instance;
//    public TMP_Text text;
//    public RectTransform rectTransform;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//        gameObject.SetActive(false);
//    }

//    public static void Show(string content)
//    {
//        if (Instance == null || string.IsNullOrEmpty(content)) return;
//        Instance.text.text = content;
//        // 🔧 Исправление: Явно конвертируем Vector2 в Vector3
//        Instance.rectTransform.anchoredPosition = (Vector3)Input.mousePosition + Vector3.up * 20;
//        Instance.gameObject.SetActive(true);
//    }

//    public static void Hide() => Instance?.gameObject.SetActive(false);

//    void Update()
//    {
//        // 🔧 Исправление: Явно конвертируем Vector2 в Vector3
//        rectTransform.anchoredPosition = (Vector3)Input.mousePosition + Vector3.up * 20;
//    }
//}
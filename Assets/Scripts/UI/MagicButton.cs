//// Assets/Scripts/UI/MagicButton.cs
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class MagicButton : MonoBehaviour
//{
//    public string spellId;
//    public Image icon;
//    public TMP_Text spellNameText;
//    public Outline outline;  // для рамки выделения

//    private Button button;
//    private CharacterSelectController controller;

//    void Awake()
//    {
//        button = GetComponent<Button>();
//    }

//    /// <summary>
//    /// Инициализация кнопки заклинания
//    /// </summary>
//    public void Setup(string id, CharacterSelectController ctrl)
//    {
//        spellId = id;
//        controller = ctrl;

//        var spell = SpellDatabase.Get(spellId);
//        if (spell != null)
//        {
//            if (icon != null && spell.icon != null)
//                icon.sprite = spell.icon;
//            if (spellNameText != null)
//                spellNameText.text = spell.displayName;
//        }

//        // Клик → выбор заклинания
//        button.onClick.RemoveAllListeners();
//        button.onClick.AddListener(() =>
//        {
//            controller?.OnSpellSelected(spellId);
//        });
//    }

//    public void Select()
//    {
//        if (outline != null)
//            outline.enabled = true;
//    }

//    public void Deselect()
//    {
//        if (outline != null)
//            outline.enabled = false;
//    }
//}
//// Scripts/UI/CharacterButton.cs
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class CharacterButton : MonoBehaviour
//{
//    [Header("References")]
//    public Image portraitImage;
//    public TMP_Text nameLabel;
//    public Outline outline;

//    private Button button;
//    private string characterId;
//    private CharacterSelectController controller;

//    void Awake()
//    {
//        button = GetComponent<Button>();
//    }

//    public void Setup(string id, CharacterSelectController ctrl)
//    {
//        characterId = id;
//        controller = ctrl;

//        var data = Resources.Load<CharacterData>($"Characters/Character_{id}");
//        // ↑ Например: "Characters/Character_Fire"
//        if (data != null)
//        {
//            if (portraitImage != null && data.portrait != null)
//                portraitImage.sprite = data.portrait;
//            if (nameLabel != null)
//                nameLabel.text = data.displayName;
//        }

//        button.onClick.RemoveAllListeners();
//        button.onClick.AddListener(() => controller?.OnCharacterSelected(characterId));
//    }

//    public void Select()
//    {
//        if (outline != null) outline.enabled = true;
//    }

//    public void Deselect()
//    {
//        if (outline != null) outline.enabled = false;
//    }
//}
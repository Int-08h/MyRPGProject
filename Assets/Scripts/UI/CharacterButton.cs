using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image portraitImage;
    public TMP_Text nameText;
    public Outline outline;

    public string charId { get; private set; }
    private System.Action<string> onSelectCallback;

    public void Initialize(CharacterData data, System.Action<string> onSelect)
    {
        charId = data.id;
        nameText.text = data.displayName;
        if (data.portrait) portraitImage.sprite = data.portrait;
        onSelectCallback = onSelect;

        // ✅ Автоматический tooltip
        tooltipContent = $"{data.displayName}\nКласс: {data.characterClass}";
    }

    private string tooltipContent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleTooltip.Show(tooltipContent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SimpleTooltip.Hide();
    }

    public void OnClick()
    {
        if (onSelectCallback != null)
        {
            onSelectCallback.Invoke(charId);
        }
    }

    public void Select()
    {
        if (outline) outline.enabled = true;
    }

    public void Deselect()
    {
        if (outline) outline.enabled = false;
    }
}
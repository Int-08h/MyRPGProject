using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MagicButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;
    public TMP_Text nameText;
    public Outline outline;

    public string spellId { get; private set; }

    private System.Action<string> onSelectCallback;
    private string tooltipContent;

    // Добавь эти методы ВНУТРЬ класса MagicButton (рядом с остальными)
    public void HandlePointerEnter(BaseEventData data)
    {
        if (data is PointerEventData pointerData)
            OnPointerEnter(pointerData);
    }

    public void HandlePointerExit(BaseEventData data)
    {
        if (data is PointerEventData pointerData)
            OnPointerExit(pointerData);
    }

    public void HandlePointerClick(BaseEventData data)
    {
        if (data is PointerEventData pointerData)
            OnPointerClick(pointerData);
    }

    public void Initialize(SpellData spell, System.Action<string> onSelect)
    {
        spellId = spell.id;
        nameText.text = spell.displayName;
        if (spell.icon) iconImage.sprite = spell.icon;
        onSelectCallback = onSelect;

        tooltipContent = $"<b>{spell.displayName}</b>\n{spell.description}";
        if (spell.bonuses?.Length > 0)
            tooltipContent += $"\n<color=#4A90E2>Бонусы:</color> {string.Join(", ", spell.bonuses)}";
    }

    public void OnPointerEnter(PointerEventData eventData) => SimpleTooltip.Show(tooltipContent);
    public void OnPointerExit(PointerEventData eventData) => SimpleTooltip.Hide();

    // ✅ Исправленный метод — теперь соответствует интерфейсу IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onSelectCallback != null)
        {
            onSelectCallback.Invoke(spellId);
        }
    }

    public void Select()
    {
        if (outline) outline.enabled = true;
        if (outline) outline.effectColor = new Color(0.29f, 0.56f, 0.89f, 1f); // #4A90E2
        if (outline) outline.effectDistance = new Vector2(4, -4);
    }

    public void Deselect()
    {
        if (outline) outline.enabled = false;
    }
}
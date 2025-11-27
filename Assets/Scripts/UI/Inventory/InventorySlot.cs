using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string itemID;
    public Image iconImage;
    public Outline outline;

    private CharacterSelectController controller;
    private ItemDatabase.ItemData itemData; // 🟢 НОВОЕ ПОЛЕ: для быстрого доступа к данным

    // ----------------------------------------------------
    // МЕТОДЫ ИНИЦИАЛИЗАЦИИ И ОБРАБОТЧИКИ СОБЫТИЙ
    // ----------------------------------------------------

    public void Initialize(string id, Sprite icon, CharacterSelectController ctrl)
    {
        itemID = id;
        if (icon) iconImage.sprite = icon;
        // Проверяем, есть ли предмет, чтобы задать цвет
        iconImage.color = string.IsNullOrEmpty(id) ? Color.clear : Color.white;
        controller = ctrl;

        // 🟢 Загружаем данные о предмете для дальнейшего использования
        itemData = ItemDatabase.GetItem(itemID);
    }

    // Вспомогательные методы, которые, вероятно, используются в EventTriggers:
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(itemID)) return;

        // Если данных нет, не можем экипировать
        if (itemData == null)
        {
            Debug.LogError($"[InventorySlot] Не найдены ItemData для ID: {itemID}");
            return;
        }

        TryEquipOrSwap(itemData.slot);
    }

    void TryEquipOrSwap(string targetSlot)
    {
        // 1. Проверяем, что слот, в который мы пытаемся экипировать, не пуст.
        if (controller.CurrentCharacter.equippedItems.TryGetValue(targetSlot, out string equippedItemID))
        {
            // Слот занят:
            controller.EquipItem(targetSlot, null); // Снимаем старый предмет

            // ✅ ИСПРАВЛЕНО: Возвращаем старый предмет в инвентарь
            controller.CurrentCharacter.inventoryItemIds.Add(equippedItemID);

            Debug.Log($"[InventorySlot] Снят предмет '{equippedItemID}' из {targetSlot} и перемещен в инвентарь.");
        }

        // 2. Сначала убираем экипируемый предмет из списка инвентаря.
        // ЭТО ВАЖНО: Мы переносим предмет из инвентаря на персонажа.
        if (controller.CurrentCharacter.inventoryItemIds.Remove(this.itemID))
        {
            Debug.Log($"[InventorySlot] Предмет {this.itemID} удален из списка инвентаря.");
        }

        // 3. Экипируем новый предмет
        controller.EquipItem(targetSlot, itemID);
        Debug.Log($"[InventorySlot] Экипирован {itemID} в {targetSlot}");

        // 4. Требуем обновления всего UI
        controller.RefreshAllUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            SimpleTooltip.Show(itemData.tooltip);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SimpleTooltip.Hide();
    }
}
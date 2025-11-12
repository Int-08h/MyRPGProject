//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using TMPro;

//public class InventorySlot : MonoBehaviour,
//    IPointerClickHandler,
//    IPointerEnterHandler,
//    IPointerExitHandler,
//    IBeginDragHandler,
//    IDragHandler,
//    IEndDragHandler,
//    IDropHandler
//{
//    // === ДАННЫЕ СЛОТА ===
//    public string slotName;          // "head", "weapon", "ring1", "inventory_0", ...
//    public string itemID;            // ID предмета, например "ring_of_focus"
//    public bool isEquipmentSlot;     // true — слот экипировки (фиксированный), false — инвентарь

//    // === UI ===
//    public Image icon;
//    public Outline outline;
//    public Color equippedColor = new Color(0.3f, 0.5f, 1.0f, 1.0f);  // синий для экипированного
//    public Color defaultColor = Color.white;

//    // === ССЫЛКИ (установите в инспекторе или через код) ===
//    public CharacterSelectController controller; // можно и через static, но лучше явный ref

//    private Sprite emptyIcon;
//    private RectTransform rectTransform;
//    private Canvas canvas;

//    void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvas = GetComponentInParent<Canvas>();
//        emptyIcon = icon.sprite;
//        RefreshVisuals();
//    }

//    // === ПОДСКАЗКИ ===
//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        if (!string.IsNullOrEmpty(itemID))
//        {
//            var item = ItemDatabase.GetItem(itemID);
//            if (item != null && !string.IsNullOrEmpty(item.tooltip))
//                SimpleTooltip.Show(item.tooltip);
//        }
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        SimpleTooltip.Hide();
//    }

//    // === КЛИК: попытка экипировать / снять ===
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        if (string.IsNullOrEmpty(itemID)) return;

//        if (isEquipmentSlot)
//        {
//            // Это слот экипировки → снять предмет
//            UnequipItem();
//        }
//        else
//        {
//            // Это инвентарь → попытка экипировать
//            TryEquipItem();
//        }
//    }

//    // === DnD ===
//    private GameObject dragPreview;
//    private Vector2 offset;

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        if (string.IsNullOrEmpty(itemID)) return;

//        dragPreview = new GameObject("DragPreview");
//        var img = dragPreview.AddComponent<Image>();
//        img.sprite = icon.sprite;
//        img.raycastTarget = false;
//        img.color = new Color(1, 1, 1, 0.7f);

//        var rect = dragPreview.AddComponent<RectTransform>();
//        rect.sizeDelta = new Vector2(64, 64);
//        rect.position = eventData.position - offset;

//        dragPreview.transform.SetParent(canvas.transform, false);
//        dragPreview.transform.SetAsLastSibling();

//        offset = eventData.position - rectTransform.position;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (dragPreview != null)
//            dragPreview.transform.position = eventData.position - offset;
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        Destroy(dragPreview);
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//        // Получаем другой слот из события
//        var otherSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
//        if (otherSlot == null || otherSlot == this) return;

//        SwapItems(otherSlot);
//    }

//    // === ЛОГИКА ===

//    public void TryEquipItem()
//    {
//        if (string.IsNullOrEmpty(itemID)) return;
//        if (controller?.currentCharacter == null) return;

//        var item = ItemDatabase.GetItem(itemID);
//        if (item == null) return;

//        // 🔒 Защита от дублирования: если уже экипирован — отмена
//        if (controller.currentCharacter.equippedItems.ContainsValue(itemID))
//        {
//            Debug.LogWarning($"[InventorySlot] Предмет '{itemID}' уже экипирован. Дублирование запрещено.");
//            return;
//        }

//        string targetSlot = item.equipmentSlot; // "ring", "weapon", "armor" и т.д.
//        if (string.IsNullOrEmpty(targetSlot))
//        {
//            Debug.LogError($"[InventorySlot] Предмет '{itemID}' не имеет equipmentSlot!");
//            return;
//        }

//        // Ищем слот экипировки с таким именем
//        var equipSlot = FindEquipmentSlot(targetSlot);
//        if (equipSlot != null)
//        {
//            // Перемещаем предмет в экипированный слот
//            equipSlot.SetItem(itemID);
//            SetItem(""); // очищаем текущий инвентарный слот
//            Save();
//        }
//        else
//        {
//            Debug.LogError($"[InventorySlot] Не найден equipment slot '{targetSlot}' на сцене.");
//        }
//    }

//    public void UnequipItem()
//    {
//        if (string.IsNullOrEmpty(itemID)) return;
//        if (controller?.currentCharacter == null) return;

//        // Ищем свободное место в инвентаре
//        var freeSlot = FindFreeInventorySlot();
//        if (freeSlot != null)
//        {
//            freeSlot.SetItem(itemID);
//            SetItem(""); // снимаем с экипировки
//            Save();
//        }
//        else
//        {
//            Debug.LogWarning("[InventorySlot] Нет свободного места в инвентаре для снятия предмета.");
//        }
//    }

//    public void SwapItems(InventorySlot other)
//    {
//        if (other == null) return;

//        // Простой обмен (без экипировки)
//        if (!isEquipmentSlot && !other.isEquipmentSlot)
//        {
//            (other.itemID, itemID) = (itemID, other.itemID);
//            other.RefreshVisuals();
//            RefreshVisuals();
//            Save(); // сохраняем оба персонажа (по факту — один и тот же)
//            return;
//        }

//        // Если один из слотов — экипировка → перенаправляем в нужный метод
//        if (isEquipmentSlot && !string.IsNullOrEmpty(other.itemID))
//        {
//            other.TryEquipItem(); // другой (инвентарь) пытается экипировать в *нас*
//        }
//        else if (other.isEquipmentSlot && !string.IsNullOrEmpty(itemID))
//        {
//            TryEquipItem(); // мы пытаемся экипировать в *него*
//        }
//        // Иначе — no-op
//    }

//    // === ВСПОМОГАТЕЛЬНЫЕ ===

//    private InventorySlot FindEquipmentSlot(string slotType)
//    {
//        // Простой способ: ищем по префиксу slotName (например: "ring1", "ring2" → slotType="ring")
//        foreach (var slot in FindObjectsOfType<InventorySlot>())
//        {
//            if (slot.isEquipmentSlot && slot.slotName.StartsWith(slotType))
//            {
//                // Проверяем, свободен ли
//                if (string.IsNullOrEmpty(slot.itemID))
//                    return slot;
//            }
//        }
//        return null;
//    }

//    private InventorySlot FindFreeInventorySlot()
//    {
//        foreach (var slot in FindObjectsOfType<InventorySlot>())
//        {
//            if (!slot.isEquipmentSlot && string.IsNullOrEmpty(slot.itemID))
//                return slot;
//        }
//        return null;
//    }

//    public void SetItem(string newID)
//    {
//        itemID = newID;
//        RefreshVisuals();
//    }

//    public void RefreshVisuals()
//    {
//        if (string.IsNullOrEmpty(itemID))
//        {
//            icon.sprite = emptyIcon;
//            outline.enabled = false;
//        }
//        else
//        {
//            var item = ItemDatabase.GetItem(itemID);
//            icon.sprite = item?.icon ?? emptyIcon;

//            if (isEquipmentSlot)
//            {
//                outline.enabled = true;
//                outline.effectColor = equippedColor;
//            }
//            else
//            {
//                outline.enabled = false;
//            }
//        }
//    }

//    private void Save()
//    {
//        if (controller?.currentCharacter != null)
//        {
//            // Обновляем equippedItems для экипированных слотов
//            if (isEquipmentSlot)
//            {
//                if (string.IsNullOrEmpty(itemID))
//                    controller.currentCharacter.equippedItems.Remove(slotName);
//                else
//                    controller.currentCharacter.equippedItems[slotName] = itemID;
//            }

//            // Сохраняем
//            SaveSystem.SaveCharacter(controller.currentCharacter);
//        }
//    }
//}
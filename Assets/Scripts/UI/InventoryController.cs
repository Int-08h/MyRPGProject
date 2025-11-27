// Assets/Scripts/UI/InventoryController.cs
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [Header("UI References")]
    public Transform inventoryGrid;
    public GameObject inventorySlotPrefab;
    public InventorySlot[] equippedSlots;

    // ✅ Ссылка на ГЛАВНЫЙ КОНТРОЛЛЕР для доступа к текущему персонажу и базам данных
    private CharacterSelectController parentController;
    public CharacterSelectController ParentController => parentController;


    public void Initialize(CharacterSelectController controller)
    {
        parentController = controller;
    }

    /// <summary>
    /// Экипирует или снимает предмет. Вызывается из InventorySlot.
    /// </summary>
    public void EquipItem(string slotName, string itemID)
    {
        CharacterSaveData currentCharacter = parentController.CurrentCharacter;

        if (string.IsNullOrEmpty(itemID))
        {
            // Снять предмет
            currentCharacter.equippedItems.Remove(slotName);
        }
        else
        {
            // Экипировать
            currentCharacter.equippedItems[slotName] = itemID;
        }

        SaveSystem.SaveCharacter(currentCharacter);
        // Обновление UI вызывается из InventorySlot (AllRefresh)
    }

    // 💡 Реализация: Обновление слотов экипировки
    public void RefreshEquippedSlotsUI()
    {
        if (parentController.CurrentCharacter == null || equippedSlots == null) return;

        foreach (var slot in equippedSlots)
        {
            if (slot == null) continue;

            string slotName = slot.gameObject.name.Replace("Slot", "").ToLower();

            if (parentController.CurrentCharacter.equippedItems.TryGetValue(slotName, out string equippedItemId))
            {
                // Используем статическую ItemDatabase (как мы делали раньше)
                var equippedItemData = ItemDatabase.GetItem(equippedItemId);
                if (equippedItemData != null)
                {
                    // Важно: передаем ссылку на главный контроллер, чтобы InventorySlot мог вызвать EquipItem()
                    slot.Initialize(equippedItemData.id, equippedItemData.icon, parentController);
                    continue;
                }
            }

            slot.Initialize(null, null, parentController);
        }
    }

    // 💡 Реализация: Обновление инвентаря
    public void RefreshInventoryUI()
    {
        // 1. Очистка старых слотов
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        // 2. Создание новых слотов на основе инвентаря
        if (parentController.CurrentCharacter == null) return;

        foreach (string id in parentController.CurrentCharacter.inventoryItemIds)
        {
            var itemData = ItemDatabase.GetItem(id);
            if (itemData != null && itemData.id != "???")
            {
                var slotObject = Instantiate(inventorySlotPrefab, inventoryGrid);
                var slotScript = slotObject.GetComponent<InventorySlot>();
                // Инициализируем слот
                slotScript.Initialize(itemData.id, itemData.icon, parentController);
            }
        }
    }
}
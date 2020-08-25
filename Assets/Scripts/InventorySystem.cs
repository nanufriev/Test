using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySystem : MonoBehaviour
{
    public ItemEventAction AddItemInInventory;
    public ItemEventAction RemoveItemFromInventory;

    [SerializeField]
    private ItemDragSystem _dragger;

    [SerializeField]
    private List<InventoryItem> _inventoryItems;
    [SerializeField]
    private InventoryButton _inventoryButton;
    [SerializeField]
    private InventoryPanel _inventoryPanel;
    [SerializeField]
    private List<ItemSlot> _droppedSlots;
    [SerializeField]
    private List<ItemSlot> _inventorySlots;

    [SerializeField, HideInInspector]
    private SaveLoadSystem _saveLoadSystem;

    private List<Item> _savedItems = new List<Item>();

    private InventoryItem _choosenItem = null;

    public void Initialize(List<InventoryItem> inventoryItems, SaveLoadSystem saveLoadSystem)
    {
        _saveLoadSystem = saveLoadSystem;

        _inventoryItems = inventoryItems;

        foreach (var item in _inventoryItems)
        {
            if (item.Item.InInventory)
                PushItemInInventory(item);
            else
                PushItemInDrop(item);

            item.OnBeginDragEvent += OnBeginDragItem;
            item.OnEndDragEvent += OnEndDragItem;
            item.OnPointerEnterEvent += OnPoinerEnterItem;
            item.OnPointerExitEvent += OnPointerExitItem;
        }

        _inventoryButton.OnPointerUp += InventoryButtonOnPointerUp;
        _dragger.OnItemDropEvent += PushItemInInventory;

        _inventoryButton.AddListener(InventorySwitchState);
    }

    private void InventoryButtonOnPointerUp()
    {
        if (_choosenItem == null)
            return;

        PushItemInDrop(_choosenItem);
    }

    private void OnPointerExitItem(InventoryItem item)
    {
        if (_choosenItem == item)
            _choosenItem = null;
    }

    private void OnPoinerEnterItem(InventoryItem item)
    {
        _choosenItem = item;
    }

    private void PushItemInDrop(InventoryItem item)
    {
        foreach (var slot in _droppedSlots)
        {
            if (slot.IsEmpty)
            {
                item.Item.InInventory = false;
                _savedItems.Remove(item.Item);
                ChangeSlot(item, slot);
                RemoveItemFromInventory?.Invoke(item.Item.ItemID);
                break;
            }
        }
    }

    private void PushItemInInventory(InventoryItem item)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty)
            {
                item.Item.InInventory = true;
                _savedItems.Add(item.Item);
                ChangeSlot(item, slot);
                AddItemInInventory?.Invoke(item.Item.ItemID);
                break;
            }
        }
    }

    private void ChangeSlot(InventoryItem item, ItemSlot slot)
    {
        item.ChangeSlot();
        item.transform.position = slot.transform.position;
        item.transform.SetParent(slot.transform);
        item.transform.localScale = Vector3.one;
        item.SetNewStartPos(slot.transform.position);
        slot.InventoryItem = item;
    }

    private void InventorySwitchState()
    {
        if (_inventoryPanel.IsShow)
            _inventoryPanel.Hide();
        else
            _inventoryPanel.Show();
    }

    private void OnApplicationQuit()
    {
        _saveLoadSystem.SaveData(_savedItems);

        foreach (var item in _inventoryItems)
        {
            item.OnBeginDragEvent -= OnBeginDragItem;
            item.OnEndDragEvent -= OnEndDragItem;
            item.OnPointerEnterEvent -= OnPoinerEnterItem;
            item.OnPointerExitEvent -= OnPointerExitItem;
        }

        _inventoryButton.OnPointerUp -= InventoryButtonOnPointerUp;

        _dragger.OnItemDropEvent -= PushItemInInventory;

        _inventoryButton.RemoveListeners(InventorySwitchState);
    }


    private void OnEndDragItem(InventoryItem item)
    {
        _dragger.EndDrag();
    }

    private void OnBeginDragItem(InventoryItem item)
    {
        _dragger.BeginDrag(item);
    }
}

public enum ActionType
{
    Add = 0,
    Remove = 1
}

[Serializable]
public class ItemEventAction : UnityEvent<int> { }
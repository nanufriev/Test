using System;
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
    private InventoryObject _inventoryObject;
    [SerializeField]
    private InventoryPanel _inventoryPanel;
    [SerializeField]
    private List<InventoryItem> _inventorySlots;
    [SerializeField]
    private Transform _dropPoint;

    private List<ItemObject> _itemObjects =  new List<ItemObject>();
    private List<Item> _savedItems = new List<Item>();
    private SaveLoadSystem _saveLoadSystem;
    private InventoryItem _choosenItem = null;

    public void Initialize(List<ItemObject> itemObjects, SaveLoadSystem saveLoadSystem)
    {
        _saveLoadSystem = saveLoadSystem;

        _itemObjects = itemObjects;

        foreach (var item in _inventorySlots)
        {
            item.Initialzie();
            item.OnPointerEnterEvent += OnPoinerEnterItem;
            item.OnPointerExitEvent += OnPointerExitItem;
        }

        foreach (var item in _itemObjects)
        {
            if (item.Item.InInventory)
            {
                PushItemInInventory(item, true);
            }
        }

        _dragger.OnItemDropEvent += OnItemDropInInventory;
        _inventoryObject.OnInventoryOpen += InventoryOpen;
        _inventoryObject.OnInventoryClose += InventoryClose;
    }

    private void OnItemDropInInventory(ItemObject item)
    {
        PushItemInInventory(item);
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

    private void DropItem(InventoryItem item, bool mute = false)
    {
        item.Item.InInventory = false;    
        _savedItems.Remove(item.Item);
        item.ItemObject.Rigidbody.velocity = Vector3.zero;
        item.ItemObject.gameObject.SetActive(true);

        if (!mute)
        {
            RemoveItemFromInventory?.Invoke(item.Item.ItemID);
            _saveLoadSystem.SaveData(_savedItems);
        }

        item.Hide();
        _choosenItem = null;
    }

    private void PushItemInInventory(ItemObject itemObj, bool mute = false)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.Item == null)
            {
                itemObj.Item.InInventory = true;
                slot.Show(itemObj.Item, itemObj);
                itemObj.gameObject.SetActive(false);
                itemObj.transform.position = _dropPoint.position;

                _savedItems.Add(itemObj.Item);

                if (!mute)
                {
                    AddItemInInventory?.Invoke(itemObj.Item.ItemID);
                    _saveLoadSystem.SaveData(_savedItems);
                }
                break;
            }
        }
    }

    private void InventoryOpen()
    {
        if (!_inventoryPanel.IsShow)
            _inventoryPanel.Show();
    }

    private void InventoryClose()
    {
        if (_inventoryPanel.IsShow)
            _inventoryPanel.Hide();

        if (_choosenItem == null)
            return;

        DropItem(_choosenItem);
    }

    private void OnApplicationQuit()
    {
        foreach (var item in _inventorySlots)
        {
            item.OnPointerEnterEvent -= OnPoinerEnterItem;
            item.OnPointerExitEvent -= OnPointerExitItem;
        }

        _dragger.OnItemDropEvent -= OnItemDropInInventory;

        _inventoryObject.OnInventoryOpen -= InventoryOpen;
        _inventoryObject.OnInventoryClose += InventoryClose;
    }
}

[Serializable]
public class ItemEventAction : UnityEvent<int> { }
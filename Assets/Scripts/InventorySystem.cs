using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
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

    private List<Item> _savedItems = new List<Item>();

    private InventoryItem _choosenItem = null;

    private void Awake()
    {
        foreach (var item in _inventoryItems)
        {
            item.OnBeginDragEvent += OnBeginDragItem;
            item.OnEndDragEvent += OnEndDragItem;
            item.OnPointerEnterEvent += OnPoinerEnterItem;
            item.OnPointerExitEvent += OnPointerExitItem;
        }

        _inventoryButton.OnPointerUp += InventoryButtonOnPointerUp;
        _dragger.OnItemDropEvent += OnItemDrop;

        _inventoryButton.AddListener(InventorySwitchState);
    }

    private void InventoryButtonOnPointerUp()
    {
        if (_choosenItem == null)
            return;

        foreach (var slot in _droppedSlots)
        {
            if (slot.IsEmpty)
            {
                _choosenItem.ChangeSlot();
                _choosenItem.transform.position = slot.transform.position;
                _choosenItem.transform.SetParent(slot.transform);
                _choosenItem.SetNewStartPos(slot.transform.position);
                slot.InventoryItem = _choosenItem;
                _savedItems.Remove(_choosenItem.Item);
                _choosenItem = null;
                break;
            }
        }
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

    private void OnItemDrop(InventoryItem item)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty)
            {
                item.ChangeSlot();
                item.transform.position = slot.transform.position;
                item.transform.SetParent(slot.transform);
                item.SetNewStartPos(slot.transform.position);
                slot.InventoryItem = item;
                _savedItems.Add(item.Item);
                break;
            }
        }
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
        foreach (var item in _inventoryItems)
        {
            item.OnBeginDragEvent -= OnBeginDragItem;
            item.OnEndDragEvent -= OnEndDragItem;
            item.OnPointerEnterEvent -= OnPoinerEnterItem;
            item.OnPointerExitEvent -= OnPointerExitItem;
        }

        _inventoryButton.OnPointerUp -= InventoryButtonOnPointerUp;

        _dragger.OnItemDropEvent -= OnItemDrop;

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

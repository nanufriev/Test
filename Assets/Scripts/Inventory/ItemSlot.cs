using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    private bool _isEmpty;
    private InventoryItem _inventoryItem;
    public InventoryItem InventoryItem
    {
        private get => _inventoryItem;
        set
        {
            _inventoryItem = value;
            if (_inventoryItem != null)
                _inventoryItem.OnChangeSlot += OnChangeItem;
        }
    }

    public bool IsEmpty => InventoryItem == null;

    private void Start()
    {
        InventoryItem = GetComponentInChildren<InventoryItem>();
    }

    private void OnChangeItem()
    {
        if (_inventoryItem == null)
            return;

        _inventoryItem.OnChangeSlot -= OnChangeItem;
        _inventoryItem = null;
    }
}

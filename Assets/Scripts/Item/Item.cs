using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    private ItemSettings _itemSettings;

    [SerializeField]
    public bool InInventory { get; set; } = false;
    public Sprite ItemIcon => _itemSettings.Icon;
    public int ItemID => _itemSettings.ItemID;
    
    public Item(ItemSettings itemSettings)
    {
        _itemSettings = itemSettings;
    }
}

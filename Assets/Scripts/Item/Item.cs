using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    private ItemSettings _itemSettings;

    [SerializeField]
    public bool InInventory { get; set; } = false;
    public Color ItemColor => _itemSettings.Color;
    public float Weight => _itemSettings.Weight;
    public int ItemID => _itemSettings.ItemID;
    
    public Item(ItemSettings itemSettings)
    {
        _itemSettings = itemSettings;
    }
}

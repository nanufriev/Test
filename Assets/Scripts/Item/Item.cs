using System;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField]
    private ItemSettings _itemSettings;

    [SerializeField]
    public bool InInventory { get; set; }
    public Sprite ItemIcon => _itemSettings.Icon;

    
    public Item(ItemSettings itemSettings)
    {
        _itemSettings = itemSettings;
    }
}

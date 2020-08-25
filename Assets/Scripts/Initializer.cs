using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryItemPrefab;
    [SerializeField]
    private InventorySystem inventorySystem;

    private List<InventoryItem> _inventoryItems = new List<InventoryItem>();

    private SaveLoadSystem _saveLoadSystem;

    private void Awake()
    {
        _saveLoadSystem = new SaveLoadSystem();

        var itemsSettings = Resources.LoadAll<ItemSettings>(string.Empty);

        var loadedItems = _saveLoadSystem.LoadData();

        Item tempItem = null;
        InventoryItem tempInventoryItem = null;

        foreach (var itemSetings in itemsSettings)
        {
            tempItem = new Item(itemSetings);
            tempItem.InInventory = loadedItems.Exists(x => x.ItemID == itemSetings.ItemID);

            tempInventoryItem = Instantiate(_inventoryItemPrefab).GetComponent<InventoryItem>();
            tempInventoryItem.Initialzie(tempItem);

            _inventoryItems.Add(tempInventoryItem);
        }

        inventorySystem.Initialize(_inventoryItems, _saveLoadSystem);
    }
}

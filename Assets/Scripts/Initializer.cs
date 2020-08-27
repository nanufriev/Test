using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemObjectPrefab;
    [SerializeField]
    private InventorySystem inventorySystem;

    private List<ItemObject> _itemObjects = new List<ItemObject>();

    private SaveLoadSystem _saveLoadSystem;

    private void Awake()
    {
        _saveLoadSystem = new SaveLoadSystem();

        var itemsSettings = Resources.LoadAll<ItemSettings>(string.Empty);

        var loadedItems = _saveLoadSystem.LoadData();

        Item tempItem = null;
        ItemObject tempItemObject = null;

        foreach (var itemSetings in itemsSettings)
        {
            tempItem = new Item(itemSetings);
            tempItem.InInventory = loadedItems.Exists(x => x.ItemID == itemSetings.ItemID);

            tempItemObject = Instantiate(_itemObjectPrefab).GetComponent<ItemObject>();

            tempItemObject.Initialize(tempItem);
            _itemObjects.Add(tempItemObject);
        }

        inventorySystem.Initialize(_itemObjects, _saveLoadSystem);
    }
}

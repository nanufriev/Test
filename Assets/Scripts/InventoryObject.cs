using System;
using UnityEngine;

public class InventoryObject : MonoBehaviour
{
    public event Action<ItemObject> OnItemDropEvent = delegate { };
    public event Action OnInventoryOpen;
    public event Action OnInventoryClose;
    [SerializeField]
    private LayerMask _inventoryLayerMask;
    private ItemObject _item;
    private RaycastHit _hitInfo;
    private Ray _ray;
    private bool _isClicked = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_isClicked)
        {
            _hitInfo = new RaycastHit();
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            _isClicked = Physics.Raycast(_ray, out _hitInfo, _inventoryLayerMask);

            if (_isClicked)
                OnInventoryOpen();
        }

        if (Input.GetMouseButtonUp(1) && _isClicked)
        {
            _isClicked = false;
            OnInventoryClose();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _item = other.GetComponent<ItemObject>();

        if (_item != null)
            OnItemDropEvent(_item);
    }
}
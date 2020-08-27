using System;
using UnityEngine;

public class ItemDragSystem : MonoBehaviour
{
    public event Action<ItemObject> OnItemDropEvent = delegate { };
    [SerializeField]
    private LayerMask _ballsLayerMask;

    [SerializeField]
    private InventoryObject[] _targets;

    [SerializeField]
    private float _forceAmount = 500;
    private RaycastHit _hitInfo;
    private Ray _ray;
    private Vector3 _originalScreenTargetPosition;
    private Vector3 _originalRigidbodyPos;
    private float _selectionDistance;
    private Vector3 _mousePositionOffset;

    public ItemObject DragItem { get; private set; }

    private void Awake()
    {
        foreach (var target in _targets)
        {
            target.OnItemDropEvent += OnItemDrop;
        }
    }

    private void OnItemDrop(ItemObject item)
    {
        OnItemDropEvent(item);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryGetItemObject();

        if (Input.GetMouseButtonUp(0) && DragItem != null)
            DragItem = null;
    }

    private void TryGetItemObject()
    {
        _hitInfo = new RaycastHit();
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hitInfo, _ballsLayerMask))
        {
            DragItem = _hitInfo.collider.GetComponent<ItemObject>();

            if (DragItem != null)
            {
                _selectionDistance = Vector3.Distance(_ray.origin, _hitInfo.point);
                _originalScreenTargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _selectionDistance));
                _originalRigidbodyPos = _hitInfo.collider.transform.position;
            }
        }
    }

    private void FixedUpdate()
    {
        if (DragItem != null)
        {
            _mousePositionOffset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _selectionDistance)) - _originalScreenTargetPosition;
            DragItem.Rigidbody.velocity = (_originalRigidbodyPos + _mousePositionOffset - DragItem.transform.position) * _forceAmount * Time.deltaTime;
        }
    }

    private void OnApplicationQuit()
    {
        foreach (var target in _targets)
        {
            target.OnItemDropEvent -= OnItemDrop;
        }
    }
}
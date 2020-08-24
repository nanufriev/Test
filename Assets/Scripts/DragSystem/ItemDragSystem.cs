using System;
using UnityEngine;

public class ItemDragSystem : MonoBehaviour
{
    public event Action<InventoryItem> OnItemDropEvent = delegate { };

    [SerializeField]
    private ItemDropTarget[] _targets;

    public InventoryItem DragItem { get; private set; }

    public bool IsDrag { get; private set; } = false;

    private void Awake()
    {
        foreach (var target in _targets)
        {
            target.OnItemDropEvent += OnItemDropEvent;
        }
    }

    public void BeginDrag(InventoryItem item)
    {
        DragItem = item;
        IsDrag = true;
    }

    public void EndDrag()
    {
        DragItem = null;
        IsDrag = false;
    }

    private void OnApplicationQuit()
    {
        foreach (var target in _targets)
        {
            target.OnItemDropEvent -= OnItemDropEvent;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropTarget : MonoBehaviour, IDropHandler
{
    public event Action<InventoryItem> OnItemDropEvent = delegate { };

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var droppedGO = eventData.pointerDrag;
        if (droppedGO != null)
        {
            var slot = droppedGO.GetComponent<InventoryItem>();

            if (slot != null)
                OnItemDropEvent(slot);
        }
    }
}
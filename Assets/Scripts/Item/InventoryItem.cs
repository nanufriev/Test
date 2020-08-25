using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<InventoryItem> OnBeginDragEvent = delegate { };
    public event Action<InventoryItem> OnEndDragEvent = delegate { };
    public event Action<InventoryItem> OnPointerEnterEvent = delegate { };
    public event Action<InventoryItem> OnPointerExitEvent = delegate { };
    public event Action OnChangeSlot = delegate { };

    private Image _itemImage;

    private Item _item;

    private Vector3 _startPos;

    public Item Item => _item;

    public void Initialzie(Item item)
    {
        _item = item;

        _itemImage = GetComponent<Image>();
        _itemImage.sprite = _item.ItemIcon;
    }

    public void ChangeSlot()
    {
        OnChangeSlot();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (_item.InInventory)
            return;

        OnEndDragEvent(this);
        transform.position = _startPos;
        _itemImage.raycastTarget = true;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (_item.InInventory)
            return;

        OnBeginDragEvent(this);
        _startPos = transform.position;
        _itemImage.raycastTarget = false;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnPointerEnterEvent(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnPointerExitEvent(this);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_item.InInventory)
            return;

        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public void SetNewStartPos(Vector3 pos)
    {
        _startPos = pos;
    }
}

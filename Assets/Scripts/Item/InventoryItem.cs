using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<InventoryItem> OnPointerEnterEvent = delegate { };
    public event Action<InventoryItem> OnPointerExitEvent = delegate { };

    private Image _itemImage;
    public Item Item { get; private set; }
    public ItemObject ItemObject { get; private set; }

    public void Initialzie()
    {
        _itemImage = GetComponent<Image>();
    }

    public void Show(Item item, ItemObject itemObject)
    {
        Item = item;
        ItemObject = itemObject;
        _itemImage.color = Item.ItemColor;
    }

    public void Hide()
    {
        Item = null;
        ItemObject = null;
        _itemImage.color = Color.white;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null && eventData.button == PointerEventData.InputButton.Left)
            OnPointerEnterEvent(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (Item != null && eventData.button == PointerEventData.InputButton.Left)
            OnPointerExitEvent(this);
    }
}

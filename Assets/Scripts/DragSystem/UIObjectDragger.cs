using UnityEngine;
using UnityEngine.UI;

public class UIObjectDragger : MonoBehaviour
{
    [SerializeField]
    private Image _itemIcon;

    public Item DragItem { get; private set; }

    public void BeginDrag(Item item)
    {
        DragItem = item;
        _itemIcon.sprite = DragItem.ItemIcon;
        _itemIcon.enabled = true;
    }

    public void EndDrag()
    { 
        _itemIcon.enabled = false;
        DragItem = default;
    }

    private void Update()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }
}

public interface IDraggableUI
{
    Sprite ItemIcon { get; }
}
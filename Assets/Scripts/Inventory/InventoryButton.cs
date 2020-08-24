using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnPointerUp = delegate { };
    public event Action OnPointerDown = delegate { };
    private Action _onClick;

    public void AddListener(Action action)
    {
        _onClick += action;
    }

    public void RemoveListeners(Action action)
    {
        _onClick -= action;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _onClick?.Invoke();
            OnPointerDown();
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _onClick?.Invoke();
            OnPointerUp();
        }
    }
}

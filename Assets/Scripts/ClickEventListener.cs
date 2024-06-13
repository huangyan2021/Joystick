using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
    IDragHandler
{
    public static ClickEventListener Get(GameObject obj)
    {
        ClickEventListener listener = obj.GetComponent<ClickEventListener>();
        if (listener == null)
        {
            listener = obj.AddComponent<ClickEventListener>();
        }

        return listener;
    }

    private System.Action<PointerEventData> _clickedHandler = null;
    private System.Action<PointerEventData> _doubleClickedHandler = null;
    private System.Action<PointerEventData> _onPointerDownHandler = null;
    private System.Action<PointerEventData> _onPointerUpHandler = null;
    private System.Action<PointerEventData> _onDragHandler = null;
    bool _isPressed = false;

    public bool IsPressed => _isPressed;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            if (_doubleClickedHandler != null)
            {
                _doubleClickedHandler(eventData);
            }
        }
        else
        {
            if (_clickedHandler != null)
            {
                _clickedHandler(eventData);
            }
        }
    }

    public void SetClickEventHandler(System.Action<PointerEventData> handler)
    {
        _clickedHandler = handler;
    }

    public void SetDoubleClickEventHandler(System.Action<PointerEventData> handler)
    {
        _doubleClickedHandler = handler;
    }

    public void SetPointerDownHandler(System.Action<PointerEventData> handler)
    {
        _onPointerDownHandler = handler;
    }

    public void SetPointerUpHandler(System.Action<PointerEventData> handler)
    {
        _onPointerUpHandler = handler;
    }

    public void SetDragHandler(System.Action<PointerEventData> handler)
    {
        _onDragHandler = handler;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        if (_onPointerDownHandler != null)
        {
            _onPointerDownHandler(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        if (_onPointerUpHandler != null)
        {
            _onPointerUpHandler(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_onDragHandler != null)
        {
            _onDragHandler(eventData);
        }
    }
}
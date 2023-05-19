using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedSliderHandler : MonoBehaviour, IDragHandler
{
    private Action<Vector2> _onDrag;
    
    public void Init(Action<Vector2> onDrag)
    {
        _onDrag = onDrag;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _onDrag?.Invoke(eventData.position - new Vector2(Screen.width / 2f, Screen.height / 2f));
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float ScaleDuration = 0.5f;
    private const float ScaleFactor = 1.2f;
        
    [SerializeField] private RectTransform _button;

    [SerializeField] private List<MenuButton> _connectedElements;

    private Vector2 _startPosition;

    private bool _connectedTriggerBlock;

    private float _scaleFactorForConnected = ScaleFactor;
    private float _decreaseScaleForConnected = 1;

    private float _scaleFactor = ScaleFactor;
    private float _decreaseScale = 1;

    public void InitConnectedElements(List<MenuButton> connectedElements)
    {
        _connectedElements = connectedElements;
    }

    public void SetScaleForConnected(float scaleFactor, float decreaseScale = 1)
    {
        _scaleFactorForConnected = scaleFactor;
        _decreaseScaleForConnected = decreaseScale;
    }

    public void SetScale(float scaleFactor = ScaleFactor, float decreaseScale = 1)
    {
        _scaleFactor = scaleFactor;
        _decreaseScale = decreaseScale;
    }
        
    public void OnPointerEnter(PointerEventData eventData)
    {
        IncreaseSize(true, _scaleFactor);
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        DecreaseSize(true, _decreaseScale);
    }

    public void ChangeConnectedTriggerBlock(bool isBlocked)
    {
        _connectedTriggerBlock = isBlocked;
    }
        
    private void IncreaseSize(bool triggerConnected, float scaleFactor = ScaleFactor)
    {
        _button.DOKill();
        _button.DOScale(scaleFactor, ScaleDuration);
        _button.DOLocalMove(_startPosition * scaleFactor, ScaleDuration);

        if (_connectedElements == null || !triggerConnected || _connectedTriggerBlock) return;
        
        foreach (MenuButton connectedElement in _connectedElements)
        {
            connectedElement.IncreaseSize(false, _scaleFactorForConnected);
        }
    }

    private void DecreaseSize(bool triggerConnected, float scaleFactor = 1)
    {
        _button.DOKill();
        _button.DOScale(scaleFactor, ScaleDuration);
        _button.DOLocalMove(_startPosition, ScaleDuration);

        if (_connectedElements == null || !triggerConnected || _connectedTriggerBlock) return;
        
        foreach (MenuButton connectedElement in _connectedElements)
        {
            connectedElement.DecreaseSize(false, _decreaseScaleForConnected);
        }
    }

    public void SynchronizeWithConnected()
    {
        if (_connectedElements == null || _connectedElements.Count == 0)
        {
            throw new Exception("Must have connected elements");
        }

        _button.DOKill();
        _button.DOScale(_connectedElements[0].transform.localScale.x, ScaleDuration);
        _button.DOLocalMove(_connectedElements[0]._startPosition, ScaleDuration);
    }

    private void Awake()
    {
        if (!_button)
        {
            _button = GetComponent<RectTransform>();

            if (!_button)
            {
                throw new Exception("Cannot find RectTransform component");
            }
        }

        _startPosition = _button.transform.localPosition;
    }
}
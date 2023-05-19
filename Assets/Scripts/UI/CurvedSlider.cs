using System;
using UnityEngine;
using UnityEngine.UI;

public class CurvedSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _handle;
    [SerializeField] private RectTransform _handleParent;
    [SerializeField] private CurvedSliderHandler _curvedSliderHandler;
    [SerializeField] private Vector2 _xRestriction;
    [SerializeField] private Vector2 _yRestriction;
    [SerializeField] private Vector2 _angleRestriction;

    [SerializeField] private Vector2 _fillImageRestriction;
    [SerializeField] private Image _fillImage;

    private float _curveRadius;
    private float _currentValue;

    private Action<float> _changedValue;

    public void Init(Action<float> changedValue, float initialValue)
    {
        _changedValue = changedValue;
        SetInitialValue(initialValue);
    }

    private void Awake()
    {
        _curvedSliderHandler.Init(OnSliderValueChanged);
        _curveRadius = (_handleParent.rect.width - _handle.rect.width) / 2;
    }

    private void SetInitialValue(float value)
    {
        float normalizedFillValue =
            Mathf.Abs(value) * (_fillImageRestriction.y - _fillImageRestriction.x) 
            + _fillImageRestriction.x;

        _fillImage.fillAmount = normalizedFillValue;
    }

    private void OnSliderValueChanged(Vector2 value)
    {
        value = value.normalized;
        value = new Vector2(
            Mathf.Clamp(value.x, _xRestriction.x, _xRestriction.y),
            Mathf.Clamp(value.y, _yRestriction.x, _yRestriction.y));

        if (value == Vector2.zero) return;

        float angle;
        angle = Mathf.Atan(value.x / value.y) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, _angleRestriction.x, _angleRestriction.y);
        
        float x = Mathf.Sin(angle * Mathf.Deg2Rad) * _curveRadius;
        float y = Mathf.Cos(angle * Mathf.Deg2Rad) * _curveRadius;

        Vector2 position;
        position = new Vector2(x, y);

        _handle.anchoredPosition = position;

        float normalizedAngle = (angle - _angleRestriction.x) / (_angleRestriction.y - _angleRestriction.x);
        float normalizedFillValue =
            Mathf.Abs(normalizedAngle) * (_fillImageRestriction.y - _fillImageRestriction.x) 
            + _fillImageRestriction.x;

        _fillImage.fillAmount = normalizedFillValue;
        
        _changedValue?.Invoke(normalizedAngle);
    }
}
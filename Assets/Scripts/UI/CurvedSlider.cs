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

    private Transform _parent;

    private Action<float> _changedValue;

    public void Init(Action<float> changedValue, float initialValue, Transform parent)
    {
        _changedValue = changedValue;
        SetInitialValue(initialValue);
        _parent = parent;
    }

    private void Awake()
    {
        _curvedSliderHandler.Init(OnSliderValueChanged);
        _curveRadius = (_handleParent.rect.width - _handle.rect.width) / 2.05f;
    }

    private void SetInitialValue(float value)
    {
        float normalizedFillValue =
            Mathf.Abs(value) * (_fillImageRestriction.y - _fillImageRestriction.x) 
            + _fillImageRestriction.x;

        _fillImage.fillAmount = normalizedFillValue;

        _handle.anchoredPosition =
            CalculatePositionByAngle(value * (_angleRestriction.y - _angleRestriction.x) + _angleRestriction.x);
    }

    private void OnSliderValueChanged(Vector2 value)
    {
        value -= (Vector2)_parent.transform.localPosition;
        value = value.normalized;
        value = transform.InverseTransformDirection(value);
        value = new Vector2(
            Mathf.Clamp(value.x, _xRestriction.x, _xRestriction.y),
            Mathf.Clamp(value.y, _yRestriction.x, _yRestriction.y));

        if (value == Vector2.zero) return;

        float angle = Mathf.Atan(value.x / value.y) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, _angleRestriction.x, _angleRestriction.y);

        _handle.localPosition = CalculatePositionByAngle(angle);

        float normalizedAngle = (angle - _angleRestriction.x) / (_angleRestriction.y - _angleRestriction.x);
        float normalizedFillValue =
            Mathf.Abs(normalizedAngle) * (_fillImageRestriction.y - _fillImageRestriction.x) 
            + _fillImageRestriction.x;

        _fillImage.fillAmount = normalizedFillValue;
        
        _changedValue?.Invoke(normalizedAngle);
    }

    private Vector2 CalculatePositionByAngle(float angle)
    {
        float x = Mathf.Sin(angle * Mathf.Deg2Rad) * _curveRadius;
        float y = Mathf.Cos(angle * Mathf.Deg2Rad) * _curveRadius;
        
        return new Vector2(x, y);
    }
}
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float ScaleDuration = 0.5f;
        private const float ScaleFactor = 1.2f;
        
        [SerializeField] private RectTransform _button;

        private Vector2 _startPosition;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            IncreaseSize();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            DecreaseSize();
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

        private void IncreaseSize()
        {
            _button.DOKill();
            _button.DOScale(ScaleFactor, ScaleDuration);
            _button.DOLocalMove(_startPosition * ScaleFactor, ScaleDuration);
        }

        private void DecreaseSize()
        {
            _button.DOKill();
            _button.DOScale(1, ScaleDuration);
            _button.DOLocalMove(_startPosition, ScaleDuration);
        }
    }
}
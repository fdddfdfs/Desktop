using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ModelMenuView : MonoBehaviour
{
    private const float TargetRotationZ = 45f;
    private const float Duration = 0.25f;
    
    [SerializeField] private GameObject _menu;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<Image> _buttonsImages;
    
    private bool _isActive;

    public void Init(ModelMenu modelMenu)
    {
        for (var i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(()=> modelMenu.ChangeModel(index));
        }
        
        _menu.SetActive(false);
    }

    public void ChangeButtonSprite(int index, Sprite sprite)
    {
        _buttonsImages[index].sprite = sprite;
    }

    public void ChangeMenuActive(Vector2 position)
    {
        if (!_isActive)
        {
            ShowMenu(position);
        }
        else
        {
            HideMenu();
        }
    }

    public void HideIfActive()
    {
        if (_isActive) HideMenu();
    }

    private void ShowMenu(Vector2 position)
    {
        _menu.transform.localPosition = position;
        _menu.SetActive(true);
        _isActive = true;
        
        _menu.transform.DOKill();
        _menu.transform.DOScale(1, Duration / 2)
            .OnComplete(() => _menu.transform.DOLocalRotate(new Vector3(0, 0, TargetRotationZ), Duration / 2));
        //_menu.transform.DOLocalRotate(new Vector3(0, 0, TargetRotationZ), Duration);
    }

    private void HideMenu()
    {
        _isActive = false;
        
        _menu.transform.DOKill();
        _menu.transform.DOScale(0, Duration);
        _menu.transform.DOLocalRotate(Vector3.zero, Duration).OnComplete(() => _menu.SetActive(false));
    }
}
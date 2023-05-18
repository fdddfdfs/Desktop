using System;
using DG.Tweening;
using UnityEngine;

public class AnimatedMenuView : MonoBehaviour
{
    private const float TargetRotationZ = 45f;
    protected const float Duration = 0.25f;
    
    [SerializeField] protected GameObject _menu;
    
    public bool IsActive { get; private set; } = true;
    
    public void ChangeMenuActive(Vector2 position, Action onComplete)
    {
        if (!IsActive)
        {
            ShowMenu(position, onComplete);
        }
        else
        {
            HideMenu(onComplete);
        }
    }

    public void HideIfActive()
    {
        if (IsActive) HideMenu(null);
    }
    
    private void ShowMenu(Vector2 position, Action onComplete)
    {
        _menu.transform.localPosition = position;
        _menu.SetActive(true);
        IsActive = true;
        
        _menu.transform.DOKill();
        _menu.transform.DOScale(1, Duration / 2).OnComplete(() =>
        {
            _menu.transform.DOLocalRotate(new Vector3(0, 0, TargetRotationZ), Duration / 2);
            onComplete.Invoke();
        });
    }

    protected void HideMenu(Action onComplete)
    {
        IsActive = false;
        
        _menu.transform.DOKill();
        _menu.transform.DOScale(0, Duration);
        _menu.transform.DOLocalRotate(Vector3.zero, Duration).OnComplete(() =>
        {
            _menu.SetActive(false);
            onComplete?.Invoke();
        });
    }
}
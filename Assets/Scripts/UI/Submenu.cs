using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Submenu<TData> : MenuWithView<SubmenuView<TData>>, IMenu 
    where TData: ScriptableObject, IIcon
{
    private TData _currentData;

    protected Submenu(
        Canvas canvas,
        string menuViewResourceName,
        List<TData> data,
        Transform parent,
        GameConfig gameConfig) 
        : base(canvas, menuViewResourceName)
    {
        _view.Init(this, gameConfig);
        
        if (parent)
        {
            _view.transform.SetSiblingIndex(parent.transform.GetSiblingIndex());
        }

        if (data == null) return;
        
        for (var i = 1; i < data.Count; i++)
        {
            _view.ChangeButton(i - 1, data[i]);
        }

        _currentData = data[0];
    }
    
    public void ChangeMenuActive(Vector2 position, Action onComplete = null)
    {
        _view.ChangeMenuActive(position, onComplete);
    }

    public virtual void HideMenu()
    {
        _view.HideIfActive();
    }

    public virtual void ButtonClicked(int buttonIndex, TData buttonData)
    {
        _view.ChangeButton(buttonIndex, _currentData);

        _currentData = buttonData;
        
        Sounds.Instance.PlaySound(0, "Click");
    }

    protected void SetCurrentData(TData data)
    {
        _currentData = data;
    }
}
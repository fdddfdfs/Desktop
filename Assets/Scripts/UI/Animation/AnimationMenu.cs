using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMenu : Submenu<AnimationMenuData>
{
    private const string AnimationMenuViewResourceName = "UI/AnimationMenuView";
    private const string AnimationSubmenuViewResourceName = "UI/AnimationSubmenuView";

    private readonly AnimationSubmenu _animationSubmenu;
    private readonly Dictionary<int, List<AnimationData>> _submenuData;
    private readonly AnimationMenuView _animationMenuView;
    
    private int _currentMenu;
    
    public AnimationMenu(
        Model model,
        Canvas canvas,
        List<AnimationMenuData> animationMenuDatas,
        List<AnimationData> animationData,
        Transform parent,
        GameConfig gameConfig) 
        : base(canvas, AnimationMenuViewResourceName, null, parent, gameConfig)
    {
        _animationSubmenu = new AnimationSubmenu(
            canvas,
            AnimationSubmenuViewResourceName,
            animationData,
            _view.transform,
            gameConfig,
            model,
            new List<MenuButton>{(_view as AnimationMenuView)?.AnimationSpeedSliderMenuButton});

        _submenuData = new Dictionary<int, List<AnimationData>>();

        if (animationData.Count % _view.Count != 0)
        {
            throw new Exception($"Cannot divide {animationData.Count} data for {_view.Count} without remainder");
        }
        
        int dataPerSection = animationData.Count / _view.Count;

        for (var i = 0; i < _view.Count; i++)
        {
            List<AnimationData> section = new();
            for (var j = 0; j < dataPerSection; j++)
            {
                section.Add(animationData[i * dataPerSection + j]);
            }

            _submenuData[i] = section;
        }
        
        for (var i = 0; i < animationMenuDatas.Count; i++)
        {
            _view.ChangeButton(i, animationMenuDatas[i]);
        }
        
        _animationSubmenu.SetButtons(_submenuData[0]);

        _animationMenuView = _view as AnimationMenuView;

        if (!_animationMenuView)
        {
            throw new Exception("SubmenuView must be AnimationMenuView");
        }
        
        _animationMenuView.InitSlider(model);
    }

    public override void ButtonClicked(int buttonIndex, AnimationMenuData buttonData)
    {
        if (_animationSubmenu.IsActive && _currentMenu != buttonIndex)
        {
            _animationSubmenu.ChangeMenuActive(_view.Position, () =>
            {
                _animationSubmenu.SetButtons(_submenuData[buttonIndex]);
                _animationSubmenu.ChangeMenuActive(_view.Position);
                _animationMenuView.IncreaseSlider();
            });
            _animationMenuView.DecreaseSlider();
            
        }
        else if (_currentMenu == buttonIndex)
        {
            _animationSubmenu.ChangeMenuActive(_view.Position);
            if (_animationSubmenu.IsActive)
            {
                _animationMenuView.IncreaseSlider();
            }
            else
            {
                _animationMenuView.DecreaseSlider();
            }
        }
        else
        {
            _animationSubmenu.SetButtons(_submenuData[buttonIndex]);
            _animationSubmenu.ChangeMenuActive(_view.Position);
            _animationMenuView.IncreaseSlider();
        }
        
        _currentMenu = buttonIndex;
        
        Sounds.Instance.PlaySound(0, "Click");
    }
    
    public override void HideMenu()
    {
        base.HideMenu();
        
        if (_animationSubmenu.IsActive)
        {
            _animationMenuView.DecreaseSlider();
            _animationSubmenu.HideMenu();
        }
    }
}
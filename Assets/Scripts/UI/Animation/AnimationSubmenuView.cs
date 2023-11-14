using System.Collections.Generic;

public class AnimationSubmenuView : SubmenuView<AnimationData>
{
    private const int AlwaysUnlocked = 2;
    private const int ConnectedElementsCount = 4;
    
    public override void Init(Submenu<AnimationData> submenu, GameConfig gameConfig)
    {
        base.Init(submenu, gameConfig);

        for (var i = 0; i < AlwaysUnlocked; i++)
        {
            _lockers[i].enabled = false;
        }
        
        for (int i = AlwaysUnlocked; i < _lockers.Count; i++)
        {
            _lockers[i].enabled = gameConfig.IsDemo;
        }
    }

    public void InitConnectedElements(List<MenuButton> connectedElements)
    {
        for (var i = 0; i < ConnectedElementsCount; i++)
        {
            var menuButton = _buttons[i].GetComponent<MenuButton>();
            menuButton.InitConnectedElements(connectedElements);
            menuButton.SetScaleForConnected(
                AnimationMenuView.SliderIncreasedScale,
                AnimationMenuView.SliderIncreasedSize);
        }
    }
}
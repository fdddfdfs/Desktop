using UnityEngine;

public class CustomizationMenuView : SubmenuView<CustomizationMenuData>
{
    [SerializeField] private CurvedSlider _topSize;
    [SerializeField] private CurvedSlider _bottomSize;

    public void Init(Model model)
    {
        _topSize.Init((newWeight) => 
            model.CustomizeWeight(CustomizationType.TopBody, newWeight), 0.5f, _menu.transform);
        _bottomSize.Init((newWeight) => 
            model.CustomizeWeight(CustomizationType.DownBody, newWeight), 0.5f, _menu.transform);
    }
}
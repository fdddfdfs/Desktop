using System.Collections.Generic;
using UnityEngine;

public class CustomizationMenu : Submenu<CustomizationMenuData>
{
    private const string CustomizationSubmenuResourceName = "UI/CustomizationSubmenuView";
    private const string CustomizationMenuResourceName = "UI/CustomizationMenuView";
    
    private readonly Model _model;
    private readonly Dictionary<ModelData, CustomizationMenuData> _customizationMenuDatas;
    private readonly CustomizationSubmenu _customizationSubmenu;
    private readonly Dictionary<ModelData, Dictionary<CustomizationType, List<CustomizationData>>> _customizationData;

    private int _currentMenu;

    public CustomizationMenu(
        Model model,
        Canvas canvas,
        List<CustomizationMenuData> data, 
        List<CustomizationData> customizationDatas,
        Transform parent) 
        : base(canvas, CustomizationMenuResourceName, null, parent)
    {
        _model = model;

        _customizationData = new Dictionary<ModelData, Dictionary<CustomizationType, List<CustomizationData>>>();
        foreach (CustomizationData customizationData in customizationDatas)
        {
            foreach (ModelData eligibleModel in customizationData.EligibleModels)
            {
                if (!_customizationData.ContainsKey(eligibleModel))
                {
                    _customizationData[eligibleModel] = new Dictionary<CustomizationType, List<CustomizationData>>();
                }

                if (!_customizationData[eligibleModel].ContainsKey(customizationData.CustomizationType))
                {
                    _customizationData[eligibleModel][customizationData.CustomizationType] =
                        new List<CustomizationData>();
                }

                _customizationData[eligibleModel][customizationData.CustomizationType].Add(customizationData);
            }
        }
        
        _customizationSubmenu = new (
            model,
            canvas,
            CustomizationSubmenuResourceName,
            _customizationData[_model.CurrentModelData][CustomizationType.Top],
            _view.transform);

        for (var i = 0; i < data.Count; i++)
        {
            _view.ChangeButton(i, data[i]);
        }
    }
    
    public override void ButtonClicked(int buttonIndex, CustomizationMenuData buttonData)
    {
        ModelData model = _model.CurrentModelData;
        CustomizationType type = buttonData.CustomizationType;

        if (_customizationSubmenu.IsActive && _currentMenu != buttonIndex)
        {
            _customizationSubmenu.ChangeMenuActive(_view.Position, () =>
            {
                _customizationSubmenu.SetButtons(model, type, _customizationData[model][type]);
                _customizationSubmenu.ChangeMenuActive(_view.Position);
            });
        }
        else if (_currentMenu == buttonIndex)
        {
            _customizationSubmenu.ChangeMenuActive(_view.Position);
        }
        else
        {
            _customizationSubmenu.SetButtons(model, type, _customizationData[model][type]);
            _customizationSubmenu.ChangeMenuActive(_view.Position);
        }
        
        _currentMenu = buttonIndex;
    }

    public override void HideMenu()
    {
        base.HideMenu();
        
        _customizationSubmenu.HideMenu();
    }
}
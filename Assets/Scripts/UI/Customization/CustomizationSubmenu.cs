using System.Collections.Generic;
using UnityEngine;

public class CustomizationSubmenu : Submenu<CustomizationData>
{
    private readonly Model _model;
    
    private Dictionary<ModelData, Dictionary<CustomizationType, CustomizationData>> _currentDatas;

    public bool IsActive => _view.IsActive;

    public CustomizationSubmenu(
        Model model,
        Canvas canvas,
        string menuViewResourceName,
        List<CustomizationData> data,
        Transform parent) 
        : base(canvas, menuViewResourceName, data, parent)
    {
        _currentDatas = new Dictionary<ModelData, Dictionary<CustomizationType, CustomizationData>>();
        _model = model;
    }

    public void SetButtons(ModelData modelData, CustomizationType customizationType, List<CustomizationData> data)
    {
        if (!_currentDatas.ContainsKey(modelData))
        {
            _currentDatas[modelData] = new Dictionary<CustomizationType, CustomizationData>();
        }

        if (!_currentDatas[modelData].ContainsKey(customizationType))
        {
            _currentDatas[modelData][customizationType] = data[0];
        }
        

        CustomizationData current = _currentDatas[modelData][customizationType];
        var counter = 0;
        for (var i = 0; i < data.Count; i++)
        {
            if (data[i] == current) continue;
            
            _view.ChangeButton(counter, data[i]);
            counter++;
        }
    }

    public override void ButtonClicked(int buttonIndex, CustomizationData buttonData)
    {
        base.ButtonClicked(buttonIndex, buttonData);
        
        _model.ChangeCustomization(buttonData);
    }
}
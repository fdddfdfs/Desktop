using System.Collections.Generic;
using UnityEngine;

public class ModelMenu : IMenu
{
    private const string ModelMenuViewResourceName = "UI/ModelMenuView";

    private readonly ModelMenuView _modelMenuView;
    private readonly Model _model;
    private readonly List<ModelData> _modelDatas;

    private int _currentModelIndex;
    
    public ModelMenu(Model model, List<ModelData> modelDatas, Canvas canvas)
    {
        _model = model;
        _modelDatas = modelDatas;
        _modelMenuView = ResourcesLoader.InstantiateLoadComponent<ModelMenuView>(ModelMenuViewResourceName);
        _modelMenuView.transform.SetParent(canvas.transform, false);
        _modelMenuView.Init(this);
        
        for (int i = 1; i < _modelDatas.Count; i++)
        {
            _modelMenuView.ChangeButtonSprite(i - 1, _modelDatas[i].Icon);
        }
    }

    public void ChangeModel(int index)
    {
        if (index >= _currentModelIndex)
        {
            index += 1;
        }
        
        _model.ChangeModel(_modelDatas[index]);

        int startIndex = Mathf.Min(index, _currentModelIndex);
        int currentIndex = startIndex;
        for (int i = startIndex; i < _modelDatas.Count; i++)
        {
            if (i == index) continue;
            _modelMenuView.ChangeButtonSprite(currentIndex, _modelDatas[i].Icon);
            currentIndex += 1;
        }
        
        _currentModelIndex = index;
    }

    public void ChangeMenuActive(Vector2 position)
    {
        _modelMenuView.ChangeMenuActive(position);
    }

    public void HideMenu()
    {
        _modelMenuView.HideIfActive();
    }
}
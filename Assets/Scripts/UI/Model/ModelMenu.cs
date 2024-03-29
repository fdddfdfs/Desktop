﻿using System.Collections.Generic;
using UnityEngine;

public class ModelMenu : Submenu<ModelData>
{
    private const string ModelMenuViewResourceName = "UI/ModelMenuView";
    
    private readonly Model _model;
    private readonly List<ModelData> _modelDatas;

    private ModelData _currentModel;

    public ModelMenu(Model model, List<ModelData> modelDatas, Canvas canvas, Transform parent, GameConfig gameConfig) 
        : base(canvas, ModelMenuViewResourceName, modelDatas, parent, gameConfig)
    {
        _model = model;
    }

    public override void ButtonClicked(int buttonIndex, ModelData buttonData)
    {
        base.ButtonClicked(buttonIndex, buttonData);

        _model.ChangeModel(buttonData);
        
        Achievements.Instance.GetAchievement(buttonData.AchievementData);
    }
}
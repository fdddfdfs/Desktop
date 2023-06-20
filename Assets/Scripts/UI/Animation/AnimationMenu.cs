using System.Collections.Generic;
using UnityEngine;

public class AnimationMenu : Submenu<AnimationData>
{
    private const string AnimationMenuViewResourceName = "UI/AnimationMenuView";

    private readonly Model _model;
    
    public AnimationMenu(Model model, Canvas canvas, List<AnimationData> animationData, Transform parent) 
        : base(canvas, AnimationMenuViewResourceName, animationData, parent)
    {
        _model = model;
    }

    public override void ButtonClicked(int buttonIndex, AnimationData buttonData)
    {
        base.ButtonClicked(buttonIndex, buttonData);
        
        _model.ChangeAnimation(buttonData.AnimationClip);
    }
}
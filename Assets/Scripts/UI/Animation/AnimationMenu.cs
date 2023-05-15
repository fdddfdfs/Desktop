using System.Collections.Generic;
using UnityEngine;

public class AnimationMenu : Submenu<AnimationData>
{
    private const string AnimationMenuViewResourceName = "UI/AnimationMenuView";

    private readonly Model _model;
    
    public AnimationMenu(Model model, Canvas canvas, List<AnimationData> animationData) 
        : base(canvas, AnimationMenuViewResourceName, animationData)
    {
        _model = model;
    }

    public override void ButtonClicked(int buttonIndex, AnimationData buttonData)
    {
        base.ButtonClicked(buttonIndex, buttonData);
        
        _model.ChangeAnimation(buttonData.AnimatorController);
    }
}
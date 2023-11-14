using System.Collections.Generic;
using UnityEngine;

public class AnimationSubmenu : Submenu<AnimationData>
{
    private readonly Model _model;

    public bool IsActive => _view.IsActive;
    
    public AnimationSubmenu(
        Canvas canvas,
        string menuViewResourceName,
        List<AnimationData> data,
        Transform parent,
        GameConfig gameConfig,
        Model model,
        List<MenuButton> connectedElements) 
        : base(canvas, menuViewResourceName, null, parent, gameConfig)
    {
        _model = model;
        _model.ChangeAnimation(data[0].AnimationClip);
        (_view as AnimationSubmenuView)?.InitConnectedElements(connectedElements);
    }
    
    public override void ButtonClicked(int buttonIndex, AnimationData buttonData)
    {
        _model.ChangeAnimation(buttonData.AnimationClip);
        
        Achievements.Instance.GetAchievement(buttonData.AchievementData);
        Sounds.Instance.PlaySound(0, "Click");
    }

    public void SetButtons(List<AnimationData> animations)
    {
        for (var i = 0; i < animations.Count; i++)
        {
            _view.ChangeButton(i, animations[i]);
        }
    }
}
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Data/AnimationData")]
public class AnimationData : ScriptableObject, IIcon
{
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Sprite _icon;

    private RuntimeAnimatorController _animatorController;

    public RuntimeAnimatorController AnimatorController
    {
        get
        {
            if (!_animatorController)
            {
                AnimatorController animatorController = new ();
                AnimatorState state = animatorController.layers[0].stateMachine.AddState(_animationClip.name);
                state.motion = _animationClip;
                _animatorController = animatorController;
            }

            return _animatorController;
        }
    }

    public Sprite Icon => _icon;
}
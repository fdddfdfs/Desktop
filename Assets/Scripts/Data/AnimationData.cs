using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Data/AnimationData")]
public class AnimationData : ScriptableObject, IIcon
{
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Sprite _icon;

    public AnimationClip AnimationClip => _animationClip;

    public Sprite Icon => _icon;
}
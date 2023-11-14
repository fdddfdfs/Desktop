using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Data/AnimationData")]
public class AnimationData : ScriptableObject, IIcon
{
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Sprite _icon;
    [SerializeField] private AchievementData _achievementData;

    public AnimationClip AnimationClip => _animationClip;

    public Sprite Icon => _icon;

    public AchievementData AchievementData => _achievementData;
}
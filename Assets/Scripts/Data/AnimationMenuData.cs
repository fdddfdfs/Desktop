using UnityEngine;

[CreateAssetMenu(fileName = "AnimationMenuData", menuName = "Data/AnimationMenuData")]
public class AnimationMenuData : ScriptableObject, IIcon
{
    [SerializeField] private Sprite _icon;

    public Sprite Icon => _icon;
}
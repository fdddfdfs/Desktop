using UnityEngine;

[CreateAssetMenu(fileName = "CustomizationMenuData", menuName = "Data/CustomizationMenuData")]
public class CustomizationMenuData : ScriptableObject, IIcon
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private CustomizationType _customizationType;

    public Sprite Icon => _icon;

    public CustomizationType CustomizationType => _customizationType;
}
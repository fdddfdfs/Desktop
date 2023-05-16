using UnityEngine;

[CreateAssetMenu(fileName = "CustomizationMenuData", menuName = "Data/CustomizationMenuData")]
public class CustomizationMenuData : ScriptableObject, IIcon
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private ModelData _modelData;
    [SerializeField] private CustomizationType _customizationType;

    public Sprite Icon => _icon;

    public ModelData ModelData => _modelData;

    public CustomizationType CustomizationType => _customizationType;
}
using UnityEngine;

[CreateAssetMenu(fileName = "ModelData", menuName = "Data/ModelData")]
public class ModelData : ScriptableObject, IIcon
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _icon;

    public GameObject Prefab => _prefab;

    public Sprite Icon => _icon;
}
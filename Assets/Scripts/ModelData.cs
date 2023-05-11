using UnityEngine;

[CreateAssetMenu(fileName = "ModelData", menuName = "ModelData")]
public class ModelData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _icon;

    public GameObject Prefab => _prefab;

    public Sprite Icon => _icon;
}
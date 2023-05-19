using System.Collections.Generic;
using UnityEngine;

public class ModelView : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private List<CustomizationMaterialIndex> _customizationMaterialsIndexes;

    private Dictionary<CustomizationType, List<int>> _customizationMaterialsIndexesByType;

    public Vector2 Size => _boxCollider.size * (Vector2)_boxCollider.transform.localScale;

    public void ChangeActive(bool active)
    {
        _model.SetActive(active);
    }

    public void ChangeAnimation(RuntimeAnimatorController runtimeAnimatorController)
    {
        _animator.runtimeAnimatorController = runtimeAnimatorController;
    }

    public void Customize(CustomizationType type, Material newMaterial)
    {
        foreach (int index in _customizationMaterialsIndexesByType[type])
        {
            _mesh.materials[index] = newMaterial;
        }
    }

    private void Awake()
    {
        _customizationMaterialsIndexesByType = new Dictionary<CustomizationType, List<int>>();
        foreach (CustomizationMaterialIndex customizationMaterialIndexes in _customizationMaterialsIndexes)
        {
            _customizationMaterialsIndexesByType[customizationMaterialIndexes.CustomizationType] =
                customizationMaterialIndexes.Index;
        }
    }
}
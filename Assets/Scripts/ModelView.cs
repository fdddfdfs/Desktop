using System.Collections.Generic;
using UnityEngine;

public class ModelView : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private List<CustomizationMesh> _customizationMeshes;

    private Dictionary<CustomizationType, List<MeshRenderer>> _customizationMeshesByType;

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
        foreach (MeshRenderer meshRenderer in _customizationMeshesByType[type])
        {
            meshRenderer.material = newMaterial;
        }
    }

    private void Awake()
    {
        _customizationMeshesByType = new Dictionary<CustomizationType, List<MeshRenderer>>();
        foreach (CustomizationMesh customizationMesh in _customizationMeshes)
        {
            if (!_customizationMeshesByType.ContainsKey(customizationMesh.CustomizationType))
            {
                _customizationMeshesByType[customizationMesh.CustomizationType] = new List<MeshRenderer>();
            }
            
            _customizationMeshesByType[customizationMesh.CustomizationType].Add(customizationMesh.MeshRenderer);
        }
    }
}
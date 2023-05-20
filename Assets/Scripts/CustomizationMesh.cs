using System;
using UnityEngine;

[Serializable]
public class CustomizationMesh
{
    [SerializeField] private CustomizationType _customizationType;
    [SerializeField] private MeshRenderer _meshRenderer;

    public CustomizationType CustomizationType => _customizationType;
    public MeshRenderer MeshRenderer => _meshRenderer;
}
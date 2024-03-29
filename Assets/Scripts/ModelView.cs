﻿using System.Collections.Generic;
using UnityEngine;

public class ModelView : MonoBehaviour
{
    private const string ResetTriggerName = "Reset";
    private const string SpeedMultiplierName = "Speed";
    
    [SerializeField] private GameObject _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private List<CustomizationMesh> _customizationMeshes;
    [SerializeField] private DynamicBone _hairsBone;

    private readonly int _resetTriggerHash = Animator.StringToHash(ResetTriggerName);
    private readonly int _speedMultiplierHash = Animator.StringToHash(SpeedMultiplierName);

    private AnimatorOverrideController _animatorOverrideController;
    private string _defaultClipName;
    private int _defaultStateName;

    private Dictionary<CustomizationType, List<CustomizationMesh>> _customizationMeshesByType;

    public Vector2 Size => _boxCollider.size * (Vector2)_boxCollider.transform.localScale;

    public Transform ModelTransform => _model.transform;

    public BoxCollider BoxCollider => _boxCollider;

    public float GetCurrentAnimationNormalizedTime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void ChangeActive(bool active)
    {
        _model.SetActive(active);
    }

    public void ChangeAnimation(AnimationClip animationClip, float normalizedTime = 0)
    {
        _animatorOverrideController[_defaultClipName] = animationClip;
        if (normalizedTime == 0)
        {
            _animator.SetTrigger(_resetTriggerHash);
        }
        else
        {
            _animator.Play(_defaultStateName, 0, normalizedTime);
        }
    }

    public void Customize(CustomizationType type, Material newMaterial)
    {
        foreach (CustomizationMesh customizationMesh in _customizationMeshesByType[type])
        {
            Material[] materials = customizationMesh.MeshRenderer.materials;
            materials[customizationMesh.Index] = newMaterial;
            customizationMesh.MeshRenderer.materials = materials;
        }
    }

    public void CustomizeHairs(HairData hairData)
    {
        foreach (HairMaterial hairMaterial in hairData.HairMaterials)
        {
            if (!_customizationMeshesByType.ContainsKey(hairMaterial.CustomizationType)) continue;
            
            foreach (CustomizationMesh customizationMesh in _customizationMeshesByType[hairMaterial.CustomizationType])
            {
                Material[] materials = customizationMesh.MeshRenderer.materials;
                materials[customizationMesh.Index] = hairMaterial.HairsMaterial;
                customizationMesh.MeshRenderer.materials = materials;
            }
        }
    }

    public void CustomizeWeight(CustomizationType type, float newWeight)
    {
        foreach (CustomizationMesh customizationMesh in _customizationMeshesByType[type])
        {
            if (type == CustomizationType.TopBody)
            {
                if (newWeight > 0.5f)
                {
                    customizationMesh.MeshRenderer.SetBlendShapeWeight(
                        customizationMesh.Index,
                        (newWeight * 2 - 1) * 100);
                    customizationMesh.MeshRenderer.SetBlendShapeWeight(
                        customizationMesh.Index + 1, 0);
                }
                else
                {
                    customizationMesh.MeshRenderer.SetBlendShapeWeight(customizationMesh.Index, 0);
                    customizationMesh.MeshRenderer.SetBlendShapeWeight(
                        customizationMesh.Index + 1, 
                        (1 - newWeight * 2) * 100);
                }
            }
            else
            {
                customizationMesh.MeshRenderer.SetBlendShapeWeight(customizationMesh.Index, newWeight * 100);
            }
        }
    }

    public void ChangeHairsBoneGravity(Vector3 newGravity)
    {
        _hairsBone.m_Gravity = newGravity;
    }

    public void ChangeAnimationSpeed(float speed)
    {
        _animator.SetFloat(_speedMultiplierHash, speed);
    }

    private void Awake()
    {
        _customizationMeshesByType = new Dictionary<CustomizationType, List<CustomizationMesh>>();
        foreach (CustomizationMesh customizationMesh in _customizationMeshes)
        {
            if (!_customizationMeshesByType.ContainsKey(customizationMesh.CustomizationType))
            {
                _customizationMeshesByType[customizationMesh.CustomizationType] = new List<CustomizationMesh>();
            }
            
            _customizationMeshesByType[customizationMesh.CustomizationType].Add(customizationMesh);
        }

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;
        _defaultClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        _defaultStateName = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

        _animator.applyRootMotion = false;
        _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }
}
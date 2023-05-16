using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomizationMaterialIndex
{
    [SerializeField] private List<int> _index;
    [SerializeField] private CustomizationType _customizationType;

    public List<int> Index => _index;

    public CustomizationType CustomizationType => _customizationType;
}
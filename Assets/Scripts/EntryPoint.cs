using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InputActionAsset _input;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<ModelData> _modelDatas;
    [SerializeField] private List<AnimationData> _animationDatas;
    [SerializeField] private List<CustomizationMenuData> _customizationMenuDatas;
    [SerializeField] private List<CustomizationData> _customizationDatas;

    private Model _model;

    private void Awake()
    {
        InputActionMap inputMap = _input.FindActionMap("Input");
        
        _model = new Model(_modelDatas, inputMap, Camera.main);
        Menu menu = new (
            inputMap,
            _canvas,
            _model,
            _modelDatas,
            _animationDatas,
            _customizationDatas,
            _customizationMenuDatas);
    }

    private void Update()
    {
        _model.Update();
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InputActionAsset _input;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<ModelData> _modelDatas;

    private Model _model;

    private void Awake()
    {
        InputActionMap inputMap = _input.FindActionMap("Input");
        
        Menu menu = new (inputMap, _canvas);
        _model = new Model(_modelDatas, inputMap, Camera.main);
    }

    private void Update()
    {
        _model.Update();
    }
}
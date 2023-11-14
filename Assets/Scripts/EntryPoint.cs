using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InputActionAsset _input;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private List<ModelData> _modelDatas;
    [SerializeField] private List<AnimationData> _animationDatas;
    [SerializeField] private List<AnimationMenuData> _animationMenuDatas;
    [SerializeField] private List<CustomizationMenuData> _customizationMenuDatas;
    [SerializeField] private List<CustomizationData> _customizationDatas;
    [SerializeField] private GameConfig _gameConfig;

    private Model _model;
    private Menu _menu;
    private EntryText _entryText;

    public GraphicRaycaster GraphicRaycaster => _menu.LanguageDropdownRaycaster;

    private void Awake()
    {
        var _ = new EnvironmentalVariables();

        InputActionMap inputMap = _input.FindActionMap("Input");
        
        _entryText = new EntryText(_canvas);
        _model = new Model(_modelDatas, inputMap, Camera.main);
        _menu = new Menu(
            inputMap,
            _canvas,
            _model,
            _modelDatas,
            _animationDatas,
            _animationMenuDatas,
            _customizationDatas,
            _customizationMenuDatas,
            _gameConfig,
            _entryText);

        SteamRichPresence steamRichPresence = new(inputMap);
    }

    private void Update()
    {
        _model.Update();
    }
}
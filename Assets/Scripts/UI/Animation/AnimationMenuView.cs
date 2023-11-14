using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnimationMenuView : SubmenuView<AnimationMenuData>
{
    public const float SliderIncreasedSize = 1.5f;
    public const float SliderIncreasedScale = 1.8f;
    private const int AlwaysUnlocked = 4;
    private const float ScaleDuration = 0.5f;
    private const float MaximumAnimationSpeed = 2f;

    [SerializeField] private CurvedSlider _animationSpeedSlider;
    [SerializeField] private List<MenuButton> _buttonsConnectedToSlider;
    [SerializeField] private TMP_Text _sliderText;

    private RectTransform _animationSpeedSliderRectTransform;
    private Vector2 _startPosition;

    public MenuButton AnimationSpeedSliderMenuButton { get; private set; }

    public override void Init(Submenu<AnimationMenuData> submenu, GameConfig gameConfig)
    {
        base.Init(submenu, gameConfig);

        for (var i = 0; i < AlwaysUnlocked; i++)
        {
            _lockers[i].enabled = false;
        }
        
        for (int i = AlwaysUnlocked; i < _lockers.Count; i++)
        {
            _lockers[i].enabled = gameConfig.IsDemo;
        }
        
        _startPosition = _animationSpeedSlider.transform.localPosition;
        _animationSpeedSliderRectTransform = _animationSpeedSlider.GetComponent<RectTransform>();
        AnimationSpeedSliderMenuButton = _animationSpeedSlider.GetComponent<MenuButton>();

        _sliderText.text = Localization.Instance[AllTexts.DanceSpeed];
        
        Localization.Instance.OnLanguageUpdated += () =>
        {
            _sliderText.text = Localization.Instance[AllTexts.DanceSpeed];
        };
    }

    public void InitSlider(Model model)
    {
        _animationSpeedSlider.Init(
            value => { model.ChangeAnimationSpeed(value * MaximumAnimationSpeed); }, 
            0.5f,
            _menu.gameObject.transform);
        model.ChangeAnimationSpeed(0.5f * MaximumAnimationSpeed);
    }

    public void IncreaseSlider()
    {
        AnimationSpeedSliderMenuButton.ChangeConnectedTriggerBlock(true);
        foreach (MenuButton buttonConnectedToSlider in _buttonsConnectedToSlider)
        {
            buttonConnectedToSlider.ChangeConnectedTriggerBlock(true);
        }
        
        _animationSpeedSliderRectTransform.DOKill();
        _animationSpeedSliderRectTransform.DOScale(SliderIncreasedSize, ScaleDuration);
        _animationSpeedSliderRectTransform.DOLocalMove(
            _startPosition * SliderIncreasedSize,
            ScaleDuration);
        AnimationSpeedSliderMenuButton.SetScale(1.8f, SliderIncreasedSize);
    }

    public void DecreaseSlider()
    {
        AnimationSpeedSliderMenuButton.ChangeConnectedTriggerBlock(false);
        foreach (MenuButton buttonConnectedToSlider in _buttonsConnectedToSlider)
        {
            buttonConnectedToSlider.ChangeConnectedTriggerBlock(false);
        }
        
        AnimationSpeedSliderMenuButton.SynchronizeWithConnected();
        AnimationSpeedSliderMenuButton.SetScale();
    }
}
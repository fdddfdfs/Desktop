using TMPro;
using UnityEngine;

public class CustomizationMenuView : SubmenuView<CustomizationMenuData>
{
    [SerializeField] private CurvedSlider _topSize;
    [SerializeField] private TMP_Text _topSizeText;
    [SerializeField] private CurvedSlider _bottomSize;
    [SerializeField] private TMP_Text _bottomSizeText;

    public void Init(Model model)
    {
        _topSize.Init(
            (newWeight) =>
            {
                model.CustomizeWeight(CustomizationType.TopBody, newWeight);
                Sounds.Instance.PlaySound(1, "Girl");
            },
            0.5f, 
            _menu.transform);
        _bottomSize.Init(
            (newWeight) =>
            {
                model.CustomizeWeight(CustomizationType.DownBody, newWeight);
                Sounds.Instance.PlaySound(1, "Girl");
            }, 
            0.5f,
            _menu.transform);

        Localization.Instance.OnLanguageUpdated += Localize;
    }

    private void Localize()
    {
        _topSizeText.text = Localization.Instance[AllTexts.TopSize];
        _bottomSizeText.text = Localization.Instance[AllTexts.DownSize];
    }
}
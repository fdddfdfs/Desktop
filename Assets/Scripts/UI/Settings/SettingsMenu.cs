using System;
using UnityEngine;

public class SettingsMenu: MenuWithView<SettingsMenuView>, IMenu
{
    private const string SettingsMenuViewResourceName = "UI/SettingsMenuView";
    
    public SettingsMenu(Canvas canvas, Transform parent) : base(canvas, SettingsMenuViewResourceName)
    {
        _view.Init(this);
        
        if (parent)
        {
            _view.transform.SetSiblingIndex(parent.transform.GetSiblingIndex());
        }
    }
    
    public void ChangeMenuActive(Vector2 position, Action onComplete)
    {
        _view.ChangeMenuActive(position, onComplete);
    }

    public void HideMenu()
    {
        _view.HideIfActive();
    }

    public void MusicVolumeUpdated(float newVolume)
    {
        SettingsStorage.MusicVolume.Value = newVolume;
        Music.Instance.ChangeMusicVolume(newVolume);
    }

    public void SoundsVolumeUpdated(float newVolume)
    {
        SettingsStorage.SoundVolume.Value = newVolume;
        Sounds.Instance.ChangeSoundsVolume(newVolume);
    }

    public void LanguageUpdated(Languages.Language newLanguage)
    {
        Localization.Instance.ChangeLanguage(newLanguage);
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : AnimatedMenuView
{
    [SerializeField] private TMP_Text _musicHeader;
    [SerializeField] private CurvedSlider _musicSlider;
    [SerializeField] private TMP_Text _musicValue;
    [SerializeField] private TMP_Text _soundsHeader;
    [SerializeField] private CurvedSlider _soundsSlider;
    [SerializeField] private TMP_Text _soundsValue;
    [SerializeField] private TMP_Text _languageHeader;
    [SerializeField] private TMP_Dropdown _languageDropdown;
    [SerializeField] private List<LanguageData> _languageData;
    [SerializeField] private Button _exitButton;
    
    private List<Languages.Language> _languages;
    private int _currentQuality;

    private SettingsMenu _settingsMenu;

    public void Init(SettingsMenu settingsMenu)
    {
        _settingsMenu = settingsMenu;
    }

    private void Awake()
    {
        float musicVolume = SettingsStorage.MusicVolume.Value;
        UpdateMusicVolume(musicVolume);
        _musicSlider.Init(UpdateMusicVolume, SettingsStorage.MusicVolume.Value);
        
        float soundVolume = SettingsStorage.SoundVolume.Value;
        UpdateSoundsVolume(soundVolume);
        _soundsSlider.Init(UpdateSoundsVolume, SettingsStorage.SoundVolume.Value);

        _languages = new List<Languages.Language>();
        List<TMP_Dropdown.OptionData> languagesOptions = new();
        for (var i = 0; i < _languageData.Count; i++)
        {
            LanguageData data = _languageData[i];
            languagesOptions.Add(new TMP_Dropdown.OptionData(data.LanguageName, data.Flag));
            _languages.Add(data.Language);
        }
        
        _languageDropdown.AddOptions(languagesOptions);
        _languageDropdown.onValueChanged.AddListener(UpdateLanguage);

        _languageDropdown.Show();
        _languageDropdown.value = _languages.FindIndex(
            language => (int)language == SettingsStorage.Localization.Value);
    }

    private void OnEnable()
    {
        Localization.Instance.OnLanguageUpdated += LocalizeText;
    }

    private void OnDisable()
    {
        Localization.Instance.OnLanguageUpdated -= LocalizeText;
    }

    private void Start()
    {
        LocalizeText();
    }

    private void LocalizeText()
    {
        _musicHeader.text = Localization.Instance[AllTexts.Music];
        _soundsHeader.text = Localization.Instance[AllTexts.Sounds];
        _languageHeader.text = Localization.Instance[AllTexts.Language];
    }

    private void UpdateMusicVolume(float newVolume)
    {
        _musicValue.text = ((int)(newVolume * 100)).ToString();
        _settingsMenu.MusicVolumeUpdated(newVolume);
    }

    private void UpdateSoundsVolume(float newVolume)
    {
        _soundsValue.text = ((int)(newVolume * 100)).ToString();
        _settingsMenu.SoundsVolumeUpdated(newVolume);
    }

    private void UpdateLanguage(int index)
    {
        _settingsMenu.LanguageUpdated(_languages[index]);
    }
}
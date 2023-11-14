using System;
using Steamworks;

public class CustomizationSubmenuView : SubmenuView<CustomizationData>
{
    private const int AlwaysUnlocked = 3;
    private const int DLCUnlocked = 1;
    
    private AppId_t? _dlcID;

    public override void Init(Submenu<CustomizationData> submenu, GameConfig gameConfig)
    {
        base.Init(submenu, gameConfig);

        for (var i = 0; i < AlwaysUnlocked; i++)
        {
            _lockers[i].enabled = false;
        }
        
        for (int i = AlwaysUnlocked; i < _lockers.Count - DLCUnlocked; i++)
        {
            _lockers[i].enabled = gameConfig.IsDemo;
        }

        string dlcID = Environment.GetEnvironmentVariable(EnvironmentalVariables.DLCEnvironmentVariable);
        _dlcID = dlcID == null ? null : new AppId_t(uint.Parse(dlcID));
        
        ChangeDLCLockersActive(true);
    }

    public void ChangeDLCLockersActive(bool active)
    {
        bool dlcAvailable = SteamManager.Initialized && _dlcID != null && SteamApps.BIsDlcInstalled(_dlcID.Value);

        active = active && !dlcAvailable;
        
        for (int i = _lockers.Count - DLCUnlocked; i < _lockers.Count; i++)
        {
            _lockers[i].enabled = active;
        }
    }
}
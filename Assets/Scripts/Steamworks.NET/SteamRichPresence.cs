using System;
using Steamworks;
using UnityEngine.InputSystem;

public class SteamRichPresence
{
    private const float InactiveTime = 5f;
    private const string ActiveStatusKey = "#Active";
    private const string InactiveStatusKey = "#Inactive";
    private const string SteamDisplayKey = "steam_display";

    public SteamRichPresence(InputActionMap inputActionMap)
    {
        var activeChecker = new ActiveChecker(inputActionMap, InactiveTime);
        activeChecker.ActiveChanged += SetActiveRichPresence;
    }

    private static void SetActiveRichPresence(bool activeState)
    {
        string statusKey = activeState ? ActiveStatusKey : InactiveStatusKey;

        if (!SteamManager.Initialized)
        {
            throw new Exception("Cannot set rich presence due to steamworks not initialized");
        }
        
        SteamFriends.SetRichPresence(SteamDisplayKey, statusKey);
    }
}
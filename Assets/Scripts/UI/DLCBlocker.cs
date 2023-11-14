using System;
using UnityEngine;
using UnityEngine.UI;

public class DLCBlocker : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        if (!_button)
        {
            _button = gameObject.GetComponentInChildren<Button>();

            if (!_button)
            {
                throw new Exception("Cannot find Button component in any child");
            }
        }

        _button.onClick.AddListener(() =>
        {
            Steamworks.SteamFriends.ActivateGameOverlayToWebPage(
                Environment.GetEnvironmentVariable(EnvironmentalVariables.DLCUrlEnvironmentVariable));
        });
    }
}
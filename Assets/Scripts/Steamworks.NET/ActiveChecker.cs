using System;
using System.Threading;
using UnityEngine.InputSystem;

public class ActiveChecker
{
    private readonly float _inactiveTime;
    private readonly CancellationToken[] _linkedTokens;
    
    private bool _currentActive;
    private CancellationTokenSource _cancellationTokenSource;

    public event Action<bool> ActiveChanged;
    
    public ActiveChecker(InputActionMap inputActionMap, float inactiveTime)
    {
        _inactiveTime = inactiveTime;
        
        InputAction leftMouse = inputActionMap["LeftMouse"];
        leftMouse.started += _ => ChangeActive();

        InputAction rightMouse = inputActionMap["RightMouse"];
        rightMouse.started += _ => ChangeActive();

        _linkedTokens = new[] { AsyncUtils.Instance.GetCancellationToken() };
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
    }

    private void ChangeActive()
    {
        if (_currentActive)
        {
            _cancellationTokenSource.Cancel();
            InactiveTimer();
            
            return;
        };
        
        _currentActive = true;
        ActiveChanged?.Invoke(true);
        InactiveTimer();
    }

    private async void InactiveTimer()
    {
        CancellationToken token = _cancellationTokenSource.Token;
        
        await AsyncUtils.Instance.Wait(_inactiveTime, token);

        if (token.IsCancellationRequested)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
            return;
        }

        _currentActive = false;
        ActiveChanged?.Invoke(false);
    }
}
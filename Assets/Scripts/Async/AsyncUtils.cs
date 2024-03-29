﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AsyncUtils : MonoBehaviour, ICancellationTokenProvider
{
    private CancellationTokenSource _source;
    private static AsyncUtils _instance;
    private static float _timeScale = 1f;

    public static AsyncUtils Instance
    {
        get
        {
            if (!_instance)
            {
                var gameObject = new GameObject(nameof(AsyncUtils));
                DontDestroyOnLoad(gameObject);
                _instance = gameObject.AddComponent<AsyncUtils>();
                _instance._source = new CancellationTokenSource();
            }

            return _instance;
        }
    }

    public static float TimeScale
    {
        get => _timeScale;
        set => _timeScale = Mathf.Clamp(value, 0f, 2f);
    }
    
    public async Task Wait(float time, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + time;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.deltaTime : Time.deltaTime * TimeScale;
            await Task.Yield();
        }
    }

    public async Task Wait(float time, CancellationToken cancellationToken, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + time;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.deltaTime : Time.deltaTime * TimeScale;
            await Task.Yield();

            if (cancellationToken.IsCancellationRequested) return;
        }
    }
    
    public async Task Wait(int timeMilliseconds, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + timeMilliseconds / 1000f;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.deltaTime : Time.deltaTime * TimeScale;
            await Task.Yield();
        }
    }

    public async Task Wait(int timeMilliseconds, CancellationToken cancellationToken, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + timeMilliseconds / 1000f;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.deltaTime : Time.deltaTime * TimeScale;
            await Task.Yield();

            if (cancellationToken.IsCancellationRequested) return;
        }
    }

    public Action<Task> EmptyTask => _ => { };

    private void OnDisable()
    {
        _source.Cancel();
    }

    public CancellationToken GetCancellationToken()
    {
        return _source.Token;
    }
}
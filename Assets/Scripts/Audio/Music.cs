﻿using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Music : ResourcesSingleton<Music, MusicResourceName>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;
    
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isPlaying;
    private CancellationToken[] _linkedTokens;

    public void ChangeMusicVolume(float newVolume)
    {
        _audioSource.volume = newVolume;
    }

    public async void PlayMusic(int musicIndex = -1)
    {
        if (_audioClips.Count == 0)
        {
            Debug.LogWarning("Music cannot play, coz here is no audio clips");
            return;
        }
        
        if (_cancellationTokenSource.IsCancellationRequested || _isPlaying)
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }
        
        if (musicIndex == -1)
        {
            musicIndex = Random.Range(0, _audioClips.Count);
        }

        _isPlaying = true;

        AudioClip clip = _audioClips[musicIndex];
        _audioSource.clip = clip;
        _audioSource.Play();

        await AsyncUtils.Instance.Wait(clip.length, _cancellationTokenSource.Token, true);

        if (_cancellationTokenSource.IsCancellationRequested) return;

        _isPlaying = false;
        PlayMusic((musicIndex + 1) % _audioClips.Count);
    }
    
    private void Awake()
    {
        if (!_audioSource)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        _linkedTokens = new[] { AsyncUtils.Instance.GetCancellationToken() };
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        PlayMusic();
    }
}

public class MusicResourceName : ResourceName
{
    public override string Name => "Audio/Music";
}
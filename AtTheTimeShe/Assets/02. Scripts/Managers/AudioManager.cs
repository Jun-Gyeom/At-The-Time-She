using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer mixer;
    
    [SerializeField] private AudioSource bgmSource;                                     // 배경음악 전용 AudioSource
    [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();    // 효과음용 AudioSource 리스트

    private float _currentMusicVolume = 1.0f;
    private float _currentSfxVolume = 1.0f;                                                
    
    [Header("SFX Channel Amount")]
    [SerializeField] private int sfxSourceCount = 5;                                    // 동시에 재생할 수 있는 효과음의 최대 수

    [Header("Settings")] 
    [SerializeField] private float musicTransitionDuration = 1.0f;                      // 배경음악 전환 시간 
    [SerializeField] [Range(0f, 1f)] private float musicDefaultVolume = 0.5f;           // 배경음악 기본 볼륨
    [SerializeField] [Range(0f, 1f)] private float sfxDefaultVolume = 0.5f;             // 효과음 기본 볼륨 

    private Dictionary<string, AudioClip> _bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();

    private new void Awake()
    {
        base.Awake();
        
        _bgmClips = ResourceManager.Instance.LoadAll<AudioClip>("Audio/BGM");
        _sfxClips = ResourceManager.Instance.LoadAll<AudioClip>("Audio/SFX");
        
        // 볼륨 초기화
        _currentMusicVolume = musicDefaultVolume;
        _currentSfxVolume = sfxDefaultVolume;
        SetBGMVolume(_currentMusicVolume);
        SetSFXVolume(_currentSfxVolume);
        
        // AudioSource 초기화
        InitializeSFXSources();
    }

    private void InitializeSFXSources()
    {
        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            sfxSources.Add(source);
        }
    }

    public void PlayBGM(string name)
    {
        if (_bgmClips.TryGetValue(name, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            
            // 부드럽게 배경음악 전환
            bgmSource.volume = 0;
            bgmSource.Play();
            DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, _currentMusicVolume, musicTransitionDuration);
        }
        else
        {
            Debug.LogError("PlayBGM: 다음 이름의 클립을 찾을 수 없습니다. >> " + name);
        }
    }

    public void StopBGM()
    {
        // 부드럽게 배경음악 전환
        DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, 0, musicTransitionDuration)
            .OnComplete(() =>
            {
                bgmSource.Stop();
                bgmSource.volume = _currentMusicVolume;
            } );
    }

    public void PlaySFX(string name)
    {
        if (_sfxClips.TryGetValue(name, out AudioClip clip))
        {
            // 사용 가능한 AudioSource 찾기
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                    return;
                }
            }
            Debug.LogWarning("모든 AudioSource가 재생 중입니다.");
        }
        else
        {
            Debug.LogError("PlaySFX: 다음 이름의 클립을 찾을 수 없습니다. >> " + name);
        }
    }

    public void SetBGMVolume(float volume)
    {
        _currentMusicVolume = volume; 
        mixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20); // 데시벨로 변환
    }

    public void SetSFXVolume(float volume)
    {
        _currentSfxVolume = volume; 
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20); // 데시벨로 변환
    }
}

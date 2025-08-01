using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum BGM
{
    ForestCradle,
    lobby,
    COUNT
}

public enum SFX
{
    ui_button_click,
    COUNT
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    [SerializeField] Transform BGMTrs;
    [SerializeField] Transform SFXTrs;

    const string AUDIO_PATH = "Audio";

    [SerializeField]
    private AudioMixer audioMixer;

    Dictionary<BGM, AudioSource> BGMPlayer = new Dictionary<BGM, AudioSource>();
    AudioSource currentBGMSource;       // Current Playing AudioSource

    Dictionary<SFX, AudioSource> SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        LoadBGMPlayer();
        LoadSFXPlayer();

        PlayBGM(BGM.ForestCradle);
    }

    // Load BGM Audio File
    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            string audioName = ((BGM)i).ToString();
            string audioPath = $"{AUDIO_PATH}/{audioName}";
            AudioClip audioClip = Resources.Load(audioPath) as AudioClip;

            if (!audioClip) continue;

            GameObject newObj = new GameObject(audioName);
            AudioSource newAudioSource = newObj.AddComponent<AudioSource>();

            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];

            newObj.transform.parent = BGMTrs;

            BGMPlayer[(BGM)i] = newAudioSource;
        }
    }

    // Load SFX Audio File
    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            string audioName = ((SFX)i).ToString();
            string audioPath = $"{AUDIO_PATH}/{audioName}";
            AudioClip audioClip = Resources.Load(audioPath) as AudioClip;

            if (!audioClip) continue;

            GameObject newObj = new GameObject(audioName);
            AudioSource newAudioSource = newObj.AddComponent<AudioSource>();

            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

            newObj.transform.parent = SFXTrs;

            SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    public void PlayBGM(BGM bgm)
    {

        if (!BGMPlayer.ContainsKey(bgm))
        {
            Debug.Log($"Doesn't exist {bgm}");
            return;
        }

        if (currentBGMSource)
        {
            currentBGMSource.Stop();
            currentBGMSource = null;
        }

        currentBGMSource = BGMPlayer[bgm];
        currentBGMSource.Play();
    }

    public void PauseBGM()
    {
        if (currentBGMSource) currentBGMSource.Pause();
    }

    public void ResumeBGM()
    {
        if (currentBGMSource) currentBGMSource.UnPause();
    }

    public void StopBGM()
    {
        if (currentBGMSource) currentBGMSource.Stop();
    }

    public void PlaySFX(SFX sfx)
    {
        if (!SFXPlayer.ContainsKey(sfx))
        {
            Debug.Log($"Doesn't exist {sfx}");
            return;
        }

        SFXPlayer[sfx].Play();
    }

    public void Mute()
    {
        foreach (var audioSourceItem in BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }

        foreach (var audioSourceItem in SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }

    public void UnMute()
    {
        foreach (var audioSourceItem in BGMPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }

        foreach (var audioSourceItem in SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
}

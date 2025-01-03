﻿using UnityEngine;

[System.Serializable]
public class AudioControlsState
{
    [SerializeField] public bool m_areControlsOpen = false;
    [SerializeField] public bool m_isAudioEnabled = false;
    [SerializeField] public Color m_waveformColor = new Color(0, 0.4f, 0.5f, 1);
    [SerializeField] public bool m_bpmGuideEnabled = false;
    [SerializeField] public float m_bpm = 60f;
    [SerializeField] public Color m_bpmGuideColor = new Color(1, 1, 1, 0.6f);
    [SerializeField] public bool m_showBeatLabels = false;
    [SerializeField] public int m_bpmGuidePrecision = 1;
    [SerializeField] public int m_latencyMilliseconds = 0;

    private AudioClip _m_audioClip;
    private AudioClip m_audioClipVolumeAdjusted;
    
    public AudioClip m_audioClip
    {
        get => _m_audioClip;
        set
        {
            if (_m_audioClip != value)
            {
                _m_audioClip = value;
                OnAudioChanged(value);
            }
        }
    }

    private void OnAudioChanged(AudioClip newClip)
    {
        m_audioClipVolumeAdjusted = AudioClipUtility.CloneClip(newClip);
    }

    public void PlayAudio(float time)
    {
        if (!m_isAudioEnabled)
        {
            return;
        }
        
        time += m_latencyMilliseconds / 1000f;
        AudioClipUtility.PlayAudioClip(_m_audioClip, time);
    }

    public void StopAudio()
    {
        AudioClipUtility.StopAudioClip(_m_audioClip);
    }

    public void RestartAudio(float time)
    {
        StopAudio();
        PlayAudio(time);
    }
}
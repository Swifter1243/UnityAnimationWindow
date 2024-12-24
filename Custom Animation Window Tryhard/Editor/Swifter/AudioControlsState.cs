using UnityEngine;

[System.Serializable]
public class AudioControlsState
{
    [SerializeField] public bool m_areControlsOpen = false;
    [SerializeField] public bool m_isAudioEnabled = false;
    [SerializeField] public AudioClip m_audioClip;
    [SerializeField] public Color m_waveformColor = new Color(0.504717f, 0.8666667f, 1, 1);

    public void PlayAudio()
    {
        AudioClipUtility.PlayAudioClip(m_audioClip);
    }
    public void PlayAudio(float time)
    {
        AudioClipUtility.PlayAudioClip(m_audioClip, time);
    }

    public void StopAudio()
    {
        AudioClipUtility.StopAudioClip(m_audioClip);
    }
}
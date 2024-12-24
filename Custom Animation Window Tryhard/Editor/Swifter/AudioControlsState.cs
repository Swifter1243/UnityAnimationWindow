using UnityEngine;

[System.Serializable]
public class AudioControlsState
{
    [SerializeField] public bool m_areControlsOpen = false;
    [SerializeField] public bool m_isAudioEnabled = false;
    [SerializeField] public AudioClip m_audioClip;
    [SerializeField] public Color m_waveformColor = new Color(0.504717f, 0.8666667f, 1, 1);
}
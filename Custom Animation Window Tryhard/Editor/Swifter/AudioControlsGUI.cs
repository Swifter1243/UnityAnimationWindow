using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;

[System.Serializable]
class AudioControlsGUI
{
    [SerializeField] public AnimationWindowState state;
    
    private const int IndentWidth = 10;
    private const int EdgeMargin = 5;
    
    private GUIStyle s_audioControlsTitle => new GUIStyle
    {
        alignment = TextAnchor.MiddleCenter,
        fontStyle = FontStyle.Bold,
        normal =
        {
            textColor = Color.white,
            background = Texture2D.grayTexture
        },
        fixedHeight = 20
    };

    private float _indent = 0;

    private void IncreaseIndent()
    {
        _indent += IndentWidth;
    }

    private void DecreaseIndent()
    {
        _indent -= IndentWidth;
    }
    
    private void DoIndent()
    {
        GUILayout.Space(_indent);
    }
    
    private void BeginHorizontal()
    {
        GUILayout.BeginHorizontal();
        DoIndent();
    }

    private void EndHorizontal()
    {
        GUILayout.Space(EdgeMargin);
        GUILayout.EndHorizontal();
    }

    private void VerticalSpace()
    {
        GUILayout.Space(10);
    }
    
    public void OnGUI(float hierarchyWidth)
    {
        _indent = EdgeMargin;
        
        AudioControlsState audioControls = state.audioControlsState;
        GUILayout.Label("Audio Controls", s_audioControlsTitle);
        
        VerticalSpace();

        BeginHorizontal();
        audioControls.m_isAudioEnabled = GUILayout.Toggle(audioControls.m_isAudioEnabled, "Audio Enabled");
        EndHorizontal();

        if (audioControls.m_isAudioEnabled)
        {
            IncreaseIndent();
            AudioControlsOnGUI(audioControls);
            DecreaseIndent();
        }

        GUILayoutUtility.GetRect(hierarchyWidth, hierarchyWidth, 0f, float.MaxValue, GUILayout.ExpandHeight(true));
    }

    private void AudioControlsOnGUI(AudioControlsState audioControls)
    {
        VerticalSpace();

        BeginHorizontal();
        GUILayout.Label("Audio Clip: ");
        audioControls.m_audioClip = EditorGUILayout.ObjectField(audioControls.m_audioClip, typeof(AudioClip), false) as AudioClip;
        EndHorizontal();

        BeginHorizontal();
        GUILayout.Label("Waveform Color: ");
        audioControls.m_waveformColor = EditorGUILayout.ColorField(audioControls.m_waveformColor);
        EndHorizontal();

        VerticalSpace();
        BeginHorizontal();
        audioControls.m_bpmGuideEnabled = GUILayout.Toggle(audioControls.m_bpmGuideEnabled, "BPM Guide");
        EndHorizontal();

        if (audioControls.m_bpmGuideEnabled)
        {
            IncreaseIndent();
            BPMGuideControlsOnGUI(audioControls);
            DecreaseIndent();
        }
    }

    private void BPMGuideControlsOnGUI(AudioControlsState audioControls)
    {
        VerticalSpace();

        BeginHorizontal();
        GUILayout.Label("BPM: ");
        audioControls.m_bpm = EditorGUILayout.FloatField(audioControls.m_bpm);
        EndHorizontal();

        BeginHorizontal();
        GUILayout.Label("Guide Color: ");
        audioControls.m_bpmGuideColor = EditorGUILayout.ColorField(audioControls.m_bpmGuideColor);
        EndHorizontal();
    }
}
using System;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;
using TimeArea = UnityEditor.Enemeteen.TimeArea;

[System.Serializable]
class AudioWaveformVisualizer
{
    [SerializeField] public AnimationWindowState state;

    public void Draw(Rect audioWaveformRect)
    {
        GL.Begin(GL.LINES);
        
        HandleUtility.ApplyWireMaterial();
        GL.Color(state.audioControlsState.m_waveformColor);

        float startX = audioWaveformRect.xMin;
        float endX = audioWaveformRect.xMax;
        float startY = audioWaveformRect.yMin;
        float endY = audioWaveformRect.yMax;
        float middle = audioWaveformRect.center.y;

        for (float x = startX; x < endX; x++)
        {
            float time = state.PixelToTime(x - audioWaveformRect.xMin);
            float sample = SampleAudioDataAtTime(time);
            
            float y1 = Mathf.Lerp(middle, startY, sample);
            float y2 = Mathf.Lerp(middle, endY, sample);
            
            DrawVerticalLineFast(x, y1, y2);
        }
        
        GL.End();
    }
    
    float SampleAudioClipAtTime(AudioClip clip, float time)
    {
        int samplePosition = Mathf.FloorToInt(AudioClipUtility.SecondsToSamplePosition(clip, time));
        float[] samples = new float[clip.channels];
        clip.GetData(samples, samplePosition);
        return Mathf.Abs(samples[0]);
    }

    private float SampleAudioDataAtTime(float time)
    {
        if (time < 0)
        {
            return 0;
        }
        
        AudioClip clip = state.audioControlsState.m_audioClip;
        if (clip != null)
        {
            if (time >= clip.length)
            {
                return 0;
            }
            
            // temporary measure to keep some consistency among peaks when moving
            time = Mathf.Round(time * 100) / 100;
            
            return SampleAudioClipAtTime(clip, time);
        }
        else
        {
            return 0.1f;
        }
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY) {
        GL.Vertex(new Vector3(x, minY, 0));
        GL.Vertex(new Vector3(x, maxY, 0));
    }
}
using System;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;

[System.Serializable]
class AudioWaveformVisualizer
{
    [SerializeField] public AnimationWindowState state;

    public void Draw(Rect audioWaveformRect)
    {
        GL.Begin(GL.QUADS);
        
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
            
            float x1 = x;
            float x2 = Math.Min(x + 1, endX);
            float y1 = Mathf.Lerp(middle, startY, sample);
            float y2 = Mathf.Lerp(middle, endY, sample);
            
            DrawQuadFast(x1, x2, y1, y2);
        }
        
        GL.End();
    }
    
    float SampleAudioClipAtTime(AudioClip clip, float time)
    {
        int sampleRate = clip.frequency;
        int channels = clip.channels;

        // Calculate the sample index based on the time
        int sampleIndex = Mathf.FloorToInt(time * sampleRate * 2);

        // Create an array to hold the samples
        float[] samples = new float[channels];
        
        // Read a small block of samples
        clip.GetData(samples, sampleIndex / channels);

        // Return the first channel's amplitude (absolute value for volume)
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

    private void DrawQuadFast(float x1, float x2, float y1, float y2)
    {
        GL.Vertex(new Vector3(x1, y1));
        GL.Vertex(new Vector3(x1, y2));
        GL.Vertex(new Vector3(x2, y2));
        GL.Vertex(new Vector3(x2, y1));
    }
}
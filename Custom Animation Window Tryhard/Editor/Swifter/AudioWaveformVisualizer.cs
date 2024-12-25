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
        float middleY = audioWaveformRect.center.y;
        float halfHeight = audioWaveformRect.height / 2;
        
        AudioClip clip = state.audioControlsState.m_audioClip;
        if (!clip)
        {
            GL.Vertex(new Vector3(startX, middleY, 0));
            GL.Vertex(new Vector3(endX, middleY, 0));
        }
        else
        {
            for (float x = startX; x < endX; x++)
            {
                float time = state.PixelToTime(x - audioWaveformRect.xMin);
                float sample = SampleAudioDataAtTime(time);

                if (sample < 0)
                {
                    continue;
                }
                
                float dist = halfHeight * sample;
                dist = Mathf.Max(dist, 1);
            
                float y1 = middleY - dist;
                float y2 = middleY + dist;
            
                DrawVerticalLineFast(x, y1, y2);
            }
        }
        
        GL.End();
    }

    private float SampleAudioDataAtTime(float time)
    {
        if (time < 0)
        {
            return -1;
        }
        
        AudioClip clip = state.audioControlsState.m_audioClip;
        if (clip)
        {
            // temporary measure to keep some consistency among peaks when moving
            time = Mathf.Round(time * 100) / 100;
            
            if (time >= clip.length)
            {
                return -1;
            }
            
            return AudioClipUtility.SampleClipAtTime(clip, time);
        }
        
        return 0;
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY) {
        GL.Vertex(new Vector3(x, minY, 0));
        GL.Vertex(new Vector3(x, maxY, 0));
    }
}
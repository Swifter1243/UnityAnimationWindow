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
            startX = Math.Max(0, state.TimeToPixel(0)) + startX;
            GL.Vertex(new Vector3(startX, middleY, 0));
            GL.Vertex(new Vector3(endX, middleY, 0));
        }
        else
        {
            for (float x = startX; x < endX; x++)
            {
                float time = state.PixelToTime(x - audioWaveformRect.xMin);
                float sample = SampleAudioDataAtTime(clip, time);

                if (sample < 0)
                {
                    continue;
                }
                
                float waveformHeight = halfHeight * sample;
                waveformHeight = Mathf.Max(waveformHeight, 1);
            
                float y1 = middleY - waveformHeight;
                float y2 = middleY + waveformHeight;
            
                DrawVerticalLineFast(x, y1, y2);
            }
        }
        
        GL.End();
    }

    private float SampleAudioDataAtTime(AudioClip clip, float time)
    {
        time = Mathf.Round(time * 100) / 100;
            
        if (time < 0 || time >= clip.length)
        {
            return -1;
        }
            
        return AudioClipUtility.SampleClipAtTime(clip, time);
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY) {
        GL.Vertex(new Vector3(x, minY, 0));
        GL.Vertex(new Vector3(x, maxY, 0));
    }
}
using System;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;
using TimeArea = UnityEditor.Enemeteen.TimeArea;

[System.Serializable]
class AudioWaveformVisualizer
{
    [SerializeField] public AnimationWindowState state;

    public const int MaxWindowSamples = 100;
    private float[] _samples = new float[MaxWindowSamples];

    public void DrawWaveform(Rect audioWaveformRect)
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
                float sample = SampleAudioDataAtPixel(audioWaveformRect, clip, x);

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

    private float SecondsToBeat(float bpm, float seconds)
    {
        return bpm / 60 * seconds;
    }

    public void DrawBPMGuide(Rect audioBPMRect)
    {
        float startTime = Mathf.Max(0, PixelToTime(audioBPMRect, audioBPMRect.xMin));
        float endTime = PixelToTime(audioBPMRect, audioBPMRect.xMax);

        float bpm = state.audioControlsState.m_bpm;
        float step = 60 / bpm;
        float startTimeBounded = Mathf.Ceil(startTime / step) * step;

        GUI.BeginGroup(audioBPMRect);
        
        GL.Begin(GL.LINES);
        HandleUtility.ApplyWireMaterial();
        GL.Color(state.audioControlsState.m_bpmGuideColor);
        
        for (float t = startTimeBounded; t < endTime; t += step)
        {
            float x = state.TimeToPixel(t);
            DrawVerticalLineFast(x, 0, audioBPMRect.height);
        }
        
        GL.End();

        if (state.audioControlsState.m_showBeatLabels)
        {
            for (float t = startTimeBounded; t < endTime; t += step)
            {
                float x = state.TimeToPixel(t);
                int beat = Mathf.RoundToInt(SecondsToBeat(bpm, t));
                GUI.Label(new Rect(x + 3, -1, 40, 20), beat.ToString());
            }
        }
        
        GUI.EndGroup();
    }

    private float PixelToTime(Rect rect, float x)
    {
        return state.PixelToTime(x - rect.xMin);
    }

    private float TimeToPixel(Rect rect, float t)
    {
        return state.TimeToPixel(t) + rect.xMin;
    }

    private float SampleAudioDataAtPixel(Rect audioWaveformRect, AudioClip clip, float x1)
    {
        float x2 = x1 + 1;
        
        float t1 = PixelToTime(audioWaveformRect, x1);
        float t2 = PixelToTime(audioWaveformRect, x2);

        if (t1 < 0 || t2 > clip.length)
        {
            return -1;
        }
        
        int p1 = AudioClipUtility.SecondsToSamplePosition(clip, t1);
        int p2 = AudioClipUtility.SecondsToSamplePosition(clip, t2);
        
        int width = p2 - p1;
        width = Math.Min(width, MaxWindowSamples);
        clip.GetData(_samples, p1);
        
        float s = 0;
        
        for (int i = 0; i < width; i++)
        {
            s += Math.Abs(_samples[i]);
        }
        s /= width;
        return Mathf.Sqrt(s);
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY) {
        if (Application.platform == RuntimePlatform.WindowsEditor) {
            x = (int)x + 0.5f;
        }
        GL.Vertex(new Vector3(x, minY, 0));
        GL.Vertex(new Vector3(x, maxY, 0));
    }
}
using System;
using UnityEditor;
using UnityEditorInternal.Enemeteen;
using UnityEngine;

[System.Serializable]
class AudioWaveformVisualizer
{
    [SerializeField] public AnimationWindowState state;

    private const int SAMPLE_WIDTH = 2;

    public void Draw(Rect audioWaveformRect)
    {
        GL.Begin(GL.QUADS);
        
        HandleUtility.ApplyWireMaterial();
        GL.Color(Color.blue);

        float startX = audioWaveformRect.xMin;
        float endX = audioWaveformRect.xMax;
        float startY = audioWaveformRect.yMin;
        float endY = audioWaveformRect.yMax;
        float middle = audioWaveformRect.center.y;

        for (float x = startX; x < endX; x += SAMPLE_WIDTH)
        {
            float time = state.PixelToTime(x - audioWaveformRect.xMin);

            if (time < 0)
            {
                continue;
            }
            
            float sample = SampleAudioDataAtTime(time);
            
            float x1 = x;
            float x2 = Math.Min(x + SAMPLE_WIDTH, endX);
            float y1 = Mathf.Lerp(middle, startY, sample);
            float y2 = Mathf.Lerp(middle, endY, sample);
            
            DrawQuadFast(x1, x2, y1, y2);
        }
        
        GL.End();
    }

    private float SampleAudioDataAtTime(float time)
    {
        return Mathf.Sin(time * 2 * Mathf.PI) * 0.5f + 0.5f;
    }

    private void DrawQuadFast(float x1, float x2, float y1, float y2)
    {
        GL.Vertex(new Vector3(x1, y1));
        GL.Vertex(new Vector3(x1, y2));
        GL.Vertex(new Vector3(x2, y2));
        GL.Vertex(new Vector3(x2, y1));
    }
}
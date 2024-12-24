using UnityEngine;

public static class AudioClipUtility
{
    public static int SecondsToSamplePosition(AudioClip clip, float seconds)
    {
       return (int)System.Math.Ceiling(clip.samples * (seconds / clip.length));
    }
}
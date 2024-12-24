using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class AudioClipUtility
{
    public static int SecondsToSamplePosition(AudioClip clip, float seconds)
    {
       return Mathf.Clamp((int)(seconds * clip.frequency), 0, clip.samples);
    }
    
    public static void PlayAudioClip(AudioClip clip)
    {
        if (clip == null) return;
        
        Type audioUtilType = typeof(Editor).Assembly.GetType("UnityEditor.AudioUtil");
        MethodInfo playMethod = audioUtilType.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public);
        playMethod?.Invoke(null, new object[] { clip, 0, false });
    }
    public static void PlayAudioClip(AudioClip clip, float time)
    {
        if (clip == null) return;
        
        int samplePosition = SecondsToSamplePosition(clip, time);
        Type audioUtilType = typeof(Editor).Assembly.GetType("UnityEditor.AudioUtil");
        MethodInfo playMethod = audioUtilType.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public);
        playMethod?.Invoke(null, new object[] { clip, samplePosition, false });
    }

    public static void StopAudioClip(AudioClip clip)
    {
        if (clip == null) return;
        
        Type audioUtilType = typeof(Editor).Assembly.GetType("UnityEditor.AudioUtil");
        MethodInfo stopMethod = audioUtilType.GetMethod("StopClip", BindingFlags.Static | BindingFlags.Public);
        stopMethod?.Invoke(null, new object[] { clip });
    }
}
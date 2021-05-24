using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    public AudioClip[] ClipVariations;
    public string SoundName;
    [Range(0, 1)] public float Volume = 1;
    [Range(-3, 3)] public float Pitch = 1;
    [Range(0, 1)] public float SpatialBlend = 1;
    [Range(0, 256)] public int Priority = 128;
    public bool Loop = false;


    public AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, ClipVariations.Length);
        
        return ClipVariations[randomIndex];
    }

    public float GetRandomPitch()
    {
        return Pitch * Random.Range(.8f, 1.2f);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SoundCreator : MonoBehaviour
{
    [SerializeField] private Sound[] allSounds;
    
    private Dictionary<string, List<WorldSound>> createdWorldSounds = new Dictionary<string, List<WorldSound>>();
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    

    private void Awake()
    {
        foreach (Sound sound in allSounds)
        {
            soundDictionary.Add(sound.SoundName, sound);
            createdWorldSounds.Add(sound.SoundName, new List<WorldSound>());
        }
    }
    
    public void PlaySound(string soundName, Vector3 position = default, Transform positionToFollow = null)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            WorldSound worldSound = new GameObject(soundName).AddComponent<WorldSound>();
            worldSound.gameObject.AddComponent<AudioSource>();
                    
            worldSound.Setup(soundDictionary[soundName], this, null);
            createdWorldSounds[soundName].Add(worldSound);
            return;
        }
        
        Debug.LogError("No Sound With Name: " + soundName);
    }

    public void StopSound(string soundName)
    {
        if (!createdWorldSounds.ContainsKey(soundName))
            return;

        foreach (WorldSound worldSound in createdWorldSounds[soundName])
        {
            worldSound.DestroySound();
        }
    }

    public bool IsPlaying(string soundName)
    {
        if (!createdWorldSounds.ContainsKey(soundName))
            return false;

        return createdWorldSounds[soundName].Count > 0;
    }

    public void RemoveCreatedSound(WorldSound worldSound) => createdWorldSounds.Remove(worldSound.CurrentSound.SoundName);
}

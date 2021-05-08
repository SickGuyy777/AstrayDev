using System;
using UnityEngine;

public class WorldSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform positionToFollow;
    private SoundCreator creator;

    public Sound CurrentSound { get; private set; }


    private void Update()
    {
        if(positionToFollow != null)
            transform.position = positionToFollow.position;
    }

    public void Setup(Sound sound, SoundCreator soundCreator, Transform aPositionToFollow = null)
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        CurrentSound = sound;
        positionToFollow = aPositionToFollow;
        creator = soundCreator;

        audioSource.clip = sound.GetRandomClip();
        audioSource.volume = sound.Volume;
        audioSource.pitch = sound.GetRandomPitch();
        audioSource.spatialBlend = sound.SpatialBlend;
        audioSource.priority = sound.Priority;
        audioSource.loop = sound.Loop;
        audioSource.Play();
        
        if(audioSource.loop == false)
            Invoke(nameof(DestroySound), audioSource.clip.length * 2);
    }

    public void DestroySound()
    {
        creator.RemoveCreatedSound(this);
        Destroy(this.gameObject);
    }
}

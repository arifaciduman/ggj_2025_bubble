using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum MusicState
    {
        normal,
        danger,
        daze
    }
    public MusicState musicState;
    public MusicState lastState;

    public AudioSource mainASource, secondASource;

    public List<AudioClip> adaptiveClips = new();

    public void PlayAnotherLoop()
    {

        if (mainASource.isPlaying && mainASource.clip.length * .75f <= mainASource.time)
        {
            if (!secondASource.isPlaying)
            {
                secondASource.clip = GetAudioClip();
                secondASource.Play();
                secondASource.time = 0;
            }
        }

        if (secondASource.isPlaying && secondASource.clip.length * .75f <= secondASource.time)
        {
            if (!mainASource.isPlaying)
            {
                mainASource.clip = GetAudioClip();
                mainASource.Play();
                mainASource.time = 0;
            }
        }
    }

    public AudioClip GetAudioClip()
    {
        return adaptiveClips[(int)musicState];
    }
}

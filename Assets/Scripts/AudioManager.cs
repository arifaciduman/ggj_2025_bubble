using System.Collections;
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

    private bool _isFirstOne, _isSecondOne;

    private void Start()
    {
        StartCoroutine(PlayAnotherLoop());
    }
    public IEnumerator PlayAnotherLoop()
    {
        while (true)
        {
            if (mainASource.isPlaying && mainASource.clip.length - mainASource.time <= 0.2f)
            {
                if (!secondASource.isPlaying && !_isSecondOne)
                {
                    secondASource.clip = GetAudioClip();
                    secondASource.Play();
                    secondASource.time = 0;
                    _isSecondOne = true;
                    yield return new WaitForSeconds(mainASource.clip.length * 0.75f);
                    _isSecondOne = false;
                }
            }

            if (secondASource.isPlaying && secondASource.clip.length - secondASource.time <= 0.2f)
            {
                if (!mainASource.isPlaying && !_isFirstOne)
                {
                    mainASource.clip = GetAudioClip();
                    mainASource.Play();
                    mainASource.time = 0;
                    _isFirstOne = true;
                    yield return new WaitForSeconds(secondASource.clip.length * 0.75f);
                    _isFirstOne = false;
                }
            }

            yield return null;
        }
    }

    public AudioClip GetAudioClip()
    {
        return adaptiveClips[(int)musicState];
    }
}

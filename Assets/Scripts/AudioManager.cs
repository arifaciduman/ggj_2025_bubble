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
            if (mainASource.isPlaying && mainASource.clip.length - mainASource.time <= 2)
            {
                if (!_isSecondOne)
                {
                    secondASource.clip = GetAudioClip();
                    secondASource.Play();
                    secondASource.time = 0;
                    _isSecondOne = true;
                    
                    System.Action _action = ()=> _isSecondOne = false;
                    DelayUtility.ExecuteAfterSeconds(_action, secondASource.clip.length * .75f);
                }
            }

            if (secondASource.isPlaying && secondASource.clip.length - secondASource.time <= 2)
            {
                if (!_isFirstOne)
                {
                    mainASource.clip = GetAudioClip();
                    mainASource.Play();
                    mainASource.time = 0;
                    _isFirstOne = true;
                    
                    System.Action _action = () =>  _isFirstOne = false;
                    DelayUtility.ExecuteAfterSeconds(_action, mainASource.clip.length * .75f);
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

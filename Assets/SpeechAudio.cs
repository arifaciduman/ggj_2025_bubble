using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpeechAudio : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public List<AudioClip> audioClipsGood;
    public List<AudioClip> audioClipsBad;

    public bool badClipsPlaying;
    public bool canPlay;
    public bool _isReadyForNext;

    private void Start()
    {
        int rand = Random.Range(0, GetAudioClip().Count);
        audioSource1.clip = audioClipsGood[rand];
        int rand1 = Random.Range(0, GetAudioClip().Count);
        while (rand == rand1)
        {
            rand1 = Random.Range(0, GetAudioClip().Count);
        }
        audioSource2.clip = audioClipsGood[rand1];
    }

    private void Update()
    {
        if (canPlay)
        {
            if (!_isReadyForNext)
            {
                if (audioSource1.isPlaying)
                {
                    if (!audioSource2.isPlaying)
                    {
                        print("in 2nd as");
                        int rand = Random.Range(0, GetAudioClip().Count);
                        audioSource2.clip = audioClipsGood[rand];
                        audioSource2.Play();
                        audioSource2.time = 0;
                        _isReadyForNext = true;
                        Action action = () => _isReadyForNext = false;
                        DelayUtility.ExecuteAfterSeconds(action,audioSource2.clip.length * .95f, true);
                    }
                }
                else
                {
                    if (!audioSource1.isPlaying)
                    {
                        print("in 1st as");
                        int rand = Random.Range(0, GetAudioClip().Count);
                        audioSource1.clip = audioClipsGood[rand];
                        audioSource1.Play();
                        audioSource1.time = 0;
                        _isReadyForNext = true;
                        Action action = () => _isReadyForNext = false;
                        DelayUtility.ExecuteAfterSeconds(action,audioSource1.clip.length * .95f, true);
                    }
                }
            }
        }
        else
        {
            audioSource1.Stop();
            audioSource2.Stop();
        }
    }

    public void PlayGood()
    {
        badClipsPlaying = false;
    }
    
    public void PlayBad()
    {
        badClipsPlaying = true;
    }
    
    public void Stop()
    {
        badClipsPlaying = false;
    }

    private List<AudioClip> GetAudioClip()
    {
        return !badClipsPlaying ? audioClipsGood: audioClipsBad; 
    }
}

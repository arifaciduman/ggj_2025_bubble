using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image vibeFillImage;
    public Slider vibeSlider;
    public GameObject starvationPanel;
    public GameObject caughtPanel;

    public TMP_Text timerText;

    public bool isTimerActive;

    public float timeStart;
    public float currentTimePassed;
    
    private void Start()
    {
        starvationPanel.SetActive(false);
        caughtPanel.SetActive(false);
        StartTimer();
    }

    private void Update()
    {
        if (isTimerActive)
        {
            Timer();
        }
    }

    public void StartTimer()
    {
        isTimerActive = true;
        timeStart = Time.time;
    }


    public void Timer()
    {
        currentTimePassed = Time.time - timeStart;
        TimerText(currentTimePassed);
    }
    
    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void TimerText(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void SetVibeFill()
    {
        //vibeFillImage.fillAmount = GameManager.Instance.vibe.currentValue / GameManager.Instance.vibe.maxValue;
        vibeSlider.value = GameManager.Instance.vibe.currentValue / GameManager.Instance.vibe.maxValue;
    }

    public void SetStarvationPanelActive()
    {
        starvationPanel.SetActive(true);
    }
    
    public void SetCaughtPanelActive()
    {
        caughtPanel.SetActive(true);
    }

    public void IncreaseScore(int amount)
    {
        timerText.text = amount.ToString();
    }
}

using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image vibeFillImage;
    public GameObject starvationPanel;
    public GameObject caughtPanel;

    public TMP_Text scoreText;

    private void Start()
    {
        starvationPanel.SetActive(false);
        caughtPanel.SetActive(false);
    }

    public void SetVibeFill()
    {
        vibeFillImage.fillAmount = GameManager.Instance.vibe.currentValue / GameManager.Instance.vibe.maxValue;
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
        scoreText.text = amount.ToString();
    }
}

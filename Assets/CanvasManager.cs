using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Image vibeFillImage;
    public GameObject starvationPanel;
    public GameObject caughtPanel;

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
}

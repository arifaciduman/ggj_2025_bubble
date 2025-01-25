using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public BubbleController BubbleController;

    public List<NPCController> otherNearNPCs = new();

    public GameObject afterPopZone;
    public GameObject patrolZone;//random açılacak olan patrol küresi

    private float _alertMultipler = .1f;//affects the chance of alert bubble
    private float _alertValue;//value that fills alert's current level
    //Summon bubble interval
    private float _minInterval = 4f;
    private float _maxInterval = 7.5f;

    private bool _isAlerted;//kırmızı bubble animasyonu için alertValue artmaya başlasın mı?

    private void Awake()
    {
        SummonBubble();
    }

    private void Update()
    {
        AfterAlert();
    }

    private void AfterAlert()
    {
        if (_isAlerted)
        {
            BubbleController.SetRedBubbleAnimMotion(GetCurrentMaxValue());
        }
    }

    private void SummonBubble()
    {
        void ResetAlertEnableBubble()
        {
            _isAlerted = false;
            _alertValue = 0;
            BubbleController.EnableBubble();
        }
        DelayUtility.ExecuteAfterSeconds(ResetAlertEnableBubble, GetInterval());
    }

    public void AfterEatingBubble()
    {
        BubbleController.StartEatenAnim();
        GameManager.Instance.vibe.currentValue += 15f;
        EnableRadar();
        SummonBubble();
    }

    public void EnableRadar()
    {
        afterPopZone.SetActive(true);
        DelayUtility.ExecuteAfterFrames(RandomizeAlert, 2);
    }

    /// <summary>
    /// Calculate Re-Summon value with current danger
    /// </summary>
    /// <returns>Get summon interval value with danger multiplier</returns>
    private float GetInterval()
    {
        float _minValue = _minInterval - GameManager.Instance.danger.DangerMultiplierForInterval();
        if (_minValue <= 0) _minValue = 0;
        float _maxValue = _maxInterval - (GameManager.Instance.danger.DangerMultiplierForInterval() / 2);
        if (_maxValue <= 3f) _maxValue = 3f;

        return Random.Range(_minValue, _maxValue);
    }

    public float CalculateMutiplier()
    {
        return _alertMultipler * GameManager.Instance.danger.currentDanger;
    }

    private void RandomizeAlert()
    {
        if (otherNearNPCs.Count > 0)
        {
            int rand = Random.Range(0, otherNearNPCs.Count);
            float chance = Random.Range(0f, 1f);
            if (chance <= GetCurrentMaxChance())
            {
                otherNearNPCs[rand]._isAlerted = true;
            }
        }
        void DelayReset()
        {
            otherNearNPCs.Clear();
            afterPopZone.SetActive(false);
        }
        DelayUtility.ExecuteAfterFrames(DelayReset, 2, true);
    }

    private float GetCurrentMaxValue()
    {
        return _alertValue + (_alertMultipler * GameManager.Instance.danger.currentDanger);
    }

    private float GetCurrentMaxChance()
    {
        return _alertMultipler * GameManager.Instance.danger.currentDanger;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public BubbleController BubbleController;

    public List<NPCController> otherNearNPCs = new();

    public GameObject afterPopZone;
    public GameObject patrolZone;//random açılacak olan patrol küresi

    private float _alertMultipler = 1f;//affects the chance of alert bubble
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
        
    }

    private void SummonBubble()
    {
        DelayUtility.ExecuteAfterSeconds(BubbleController.EnableBubble, GetInterval());
    }

    public void AfterEatingBubble()
    {
        BubbleController.StartEatenAnim();
        GameManager.Instance.vibe.currentValue += 15f;
        SummonBubble();
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
}

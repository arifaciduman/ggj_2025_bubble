using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public BubbleController BubbleController;

    public List<NPCController> otherNearNPCs = new();

    public GameObject afterPopZone;
    public GameObject patrolZone;//random açılacak olan patrol küresi

    private float _alertMultipler = .1f;//affects the chance of alert bubble
    //Summon bubble interval
    private float _minInterval = 4f;
    private float _maxInterval = 7.5f;

    public bool isAlerted;//kırmızı bubble animasyonu için alertValue artmaya başlasın mı?
    private bool _isRandomized;

    private void Start()
    {
        SummonBubble();
    }

    private void Update()
    {
        AfterAlert();

        RandomizeAlert();
    }

    //Alarm çaldıktan sonrası
    private void AfterAlert()
    {
        if (isAlerted)
        {
            BubbleController.SetRedBubbleAnimMotion();
        }
    }

    private void SummonBubble()
    {
        void ResetAlertEnableBubble()
        {
            isAlerted = false;
            BubbleController.EnableBubble();
        }
        DelayUtility.ExecuteAfterSeconds(ResetAlertEnableBubble, GetInterval());
    }

    public void AfterEatingBubble()
    {
        BubbleController.StartEatenAnim();
        GameManager.Instance.vibe.currentValue += 15f;
        if (isAlerted)
        {
            GameManager.Instance.danger.currentDanger--;
        }
        EnableRadar();
        SummonBubble();
    }

    public void EnableRadar()
    {
        afterPopZone.SetActive(true);
        //RandomizeAlert();
    }

    /// <summary>
    /// Calculate Re-Summon value with current danger
    /// </summary>
    /// <returns>Get summon interval value with danger multiplier</returns>
    private float GetInterval()
    {
        float _minValue = _minInterval - GameManager.Instance.danger.currentDanger;
        if (_minValue <= 0) _minValue = 0;
        float _maxValue = _maxInterval - (GameManager.Instance.danger.currentDanger/ 2);
        if (_maxValue <= 3f) _maxValue = 3f;

        return Random.Range(_minValue, _maxValue);
    }

    private void RandomizeAlert()
    {
        if (otherNearNPCs.Count > 0 && !_isRandomized)
        {
            int rand = Random.Range(0, otherNearNPCs.Count);
            if (!otherNearNPCs[rand].isAlerted)
            {
                float chance = Random.Range(0.0f, 1.0f);

                if (chance <= GetCurrentMaxChance())
                {
                    otherNearNPCs[rand].isAlerted = true;
                    GameManager.Instance.danger.currentDanger++;
                }
                _isRandomized = true;

                void DelayReset()
                {
                    otherNearNPCs.Clear();
                    afterPopZone.SetActive(false);
                    _isRandomized = false;
                }
                DelayUtility.ExecuteAfterSeconds(DelayReset, .5f, true);
            }
        }
    }

    private float GetCurrentMaxChance()
    {
        return _alertMultipler * GameManager.Instance.danger.currentDanger;
    }
}

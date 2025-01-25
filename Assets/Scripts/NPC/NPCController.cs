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

    public Animator anim;

    public bool isAlerted;//kırmızı bubble animasyonu için alertValue artmaya başlasın mı?
    private bool _isRandomized;

    private float currentDelayCd;

    private void Start()
    {
        SummonBubble();
    }

    private void Update()
    {
        AfterAlert();

        RandomizeAlert();

        currentDelayCd -= Time.deltaTime;
        if(currentDelayCd < 0)
        {
            RandomAnimInt();
            anim.SetBool("CanTransition", true);
            currentDelayCd = Random.Range(5, 15);
            Invoke("ResetBool", 0.5f);
        }
    }

    //Alarm çaldıktan sonrası
    private void AfterAlert()
    {
        if (isAlerted)
        {
            BubbleController.SetRedBubbleAnimMotion(GetCurrentMaxValue());
        }
    }

    private void SummonBubble()
    {
        void ResetAlertEnableBubble()
        {
            isAlerted = false;
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
        //RandomizeAlert();
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

    private void RandomizeAlert()
    {
        if (otherNearNPCs.Count > 0 && !_isRandomized)
        {
            int rand = Random.Range(0, otherNearNPCs.Count);
            float chance = Random.Range(0.0f, 1.0f);

            if (chance <= GetCurrentMaxChance())
            {
                print("got the chance to alert");
                otherNearNPCs[rand].isAlerted = true;
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

    private float GetCurrentMaxValue()
    {
        return _alertValue + GetCurrentMaxChance();
    }

    private float GetCurrentMaxChance()
    {
        return _alertMultipler * GameManager.Instance.danger.currentDanger;
    }

    private void RandomAnimInt()
    {
        anim.SetInteger("RandomAnim", Random.Range(0, 4));
    }

    private void ResetBool()
    {
        anim.SetBool("CanTransition", false);
    }

}

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class NPCController : MonoBehaviour
{
    public BubbleController BubbleController;
    public SpeechAudio SpeechAudio;
    public NPCFeedbacks NpcFeedbacks;

    public List<NPCController> otherNearNPCs = new();

    public GameObject afterPopZone;
    public GameObject patrolZone;//random açılacak olan patrol küresi
    public GameObject patrolIndicator;
    public GameObject hahaObj;

    public MMF_Player patrolFeedback;//TODO: Feedbacks => SetActive(true) - ToDestination 15f - Pause 5sn -  ToDestination 1.5f - SetActive(false)

    private float _alertMultipler = .1f;//affects the chance of alert bubble
    //Summon bubble interval
    public float _minInterval = 4f;
    public float _maxInterval = 7.5f;
    private float _patrolTimer;

    public float minPatrolTimer = 15f, maxPatrolTimer = 25f;

    public float eatHealAmount = 10f;
    public float hahaChance = 0.01f;
    public bool isLaughing = false;

    public bool isAlerted;//kırmızı bubble animasyonu için alertValue artmaya başlasın mı?
    private bool _isRandomized;
    public bool tryPatrol;
    public bool canPatrol;
    private bool _enablePopZoneForPatrol;

    public Transform aimObject;
    public MultiAimConstraint headConstraint;
    public MultiAimConstraint chestConstraint;

    public Animator anim;
    private float currentDelayCd;
    
    
    private void Awake()
    {
        PlayNextAnim();
    }

    private void Start()
    {
        SummonBubble();
    }

    private void Update()
    {
        if (Random.value < hahaChance && !isLaughing)
        {
            isLaughing = true;
            StartCoroutine(HahaActivate());
        }
        
        AfterAlert();

        RandomizeAlert();

        PlayNextAnim();

        TryPatroling();
    }

    //TODO: PATROL
    private void TryPatroling()
    {
        if (!canPatrol && BubbleController.bubbleImage.localScale == Vector3.one 
                       && BubbleController.bubbleRedImage.localScale != Vector3.one && GameManager.Instance.patrolledNPC == null)
        {
            if (_patrolTimer < Random.Range(minPatrolTimer, maxPatrolTimer))
            {
                _patrolTimer += Time.deltaTime;
            }
            else
            {
                tryPatrol = true;

                if (!_enablePopZoneForPatrol)
                {
                    EnableRadar();
                    _enablePopZoneForPatrol = true;
                }
            }
        }
        else
        {
            _patrolTimer = 0;
        }
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
        DelayUtility.ExecuteAfterSeconds(BubbleController.EnableBubble, GetInterval());
    }

    public void AfterEatingBubble()
    {
        BubbleController.StartEatenAnim();
        GameManager.Instance.vibe.currentValue += eatHealAmount;
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

                if (chance <= GetCurrentMaxChance()/2)//chance
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
    
    private void RandomAnimInt()
    {
        anim.SetInteger("RandomAnim", Random.Range(0, 4));
    }

    private void ResetBool()
    {
        anim.SetBool("CanTransition", false);
    }

    private void PlayNextAnim()
    {
        currentDelayCd -= Time.deltaTime;
        if(currentDelayCd < 0)
        {
            RandomAnimInt();
            anim.SetBool("CanTransition", true);
            currentDelayCd = Random.Range(5, 15);
            DelayUtility.ExecuteAfterSeconds(ResetBool, 0.5f, true);
        }
    }

    private IEnumerator HahaActivate()
    {
        hahaObj.SetActive(true);
        
        yield return new WaitForSeconds(1.5f);

        isLaughing = false;
        hahaObj.SetActive(false);
    }

    public void PlayerDied()
    {
        aimObject.SetPositionAndRotation(GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
        headConstraint.weight = 1f;
        chestConstraint.weight = 0.33f;
    }

}

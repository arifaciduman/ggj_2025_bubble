using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;
    public CanvasManager canvasManager;

    public Vibe vibe;
    public Danger danger;

    public bool isGameOver;

    private float _satiatedTimer;//if more than designated value, isGameOver true
    private float _reduceVibeTimer;
    
    private void Awake() 
    {
        vibe = new();
        danger = new();

        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Update()
    {
        if (_reduceVibeTimer < .1f)
        {
            _reduceVibeTimer += Time.deltaTime;
        }
        else
        {
            vibe.CurrentVibeValue();
            _reduceVibeTimer = 0;
        }

        Hunger();
    }

    private void Hunger()
    {
        if (vibe.isHungry)
        {
            _satiatedTimer += Time.deltaTime;

            if (_satiatedTimer >= 5f)
            {
                isGameOver = true;
                Debug.Log("dead");
            }
        }
        else
        {
            _satiatedTimer = 0f;
        }
    }
}

[Serializable]
public class Vibe
{
    public float currentValue = 25f;
    public float maxValue = 100f;
    public float reduceValue = 1f;
    public float reduceMultiplier = 1f;

    public bool isFull;
    public bool isHungry;
    private bool _isDazeState;

    public void CurrentVibeValue()
    {
        if(currentValue > 0)
        {
            if (currentValue < maxValue || _isDazeState)
            {
                //Debug.Log($"current reduce: {reduceValue * CalculatedMultiplier() * Time.deltaTime}");
                currentValue -= reduceValue * CalculatedMultiplier();
                GameManager.Instance.canvasManager.SetVibeFill();
                isHungry = false;
                isFull = false;
            }
            else
            {
                currentValue = maxValue;
                GameManager.Instance.canvasManager.SetVibeFill();
                
                if (!_isDazeState)
                {
                    reduceMultiplier += 7.5f;
                    GameManager.Instance.player.StartDazeState();
                    void DelayDazeEnding()
                    {
                        GameManager.Instance.player.EndDazeState();
                        _isDazeState = false;
                        reduceMultiplier -= 7.5f;
                    }
                    DelayUtility.ExecuteAfterSeconds(DelayDazeEnding, 4f, true);
                    _isDazeState = true;
                }
            }
        }
        else
        {
            currentValue = 0;
            GameManager.Instance.canvasManager.SetVibeFill();
            isHungry = true;
        }
    }

    private float CalculatedMultiplier()
    {
        return reduceMultiplier * GameManager.Instance.player.stateVibeReduceMultiplier * GameManager.Instance.danger.currentDanger;
    }
}

[Serializable]
public class Danger
{
    public float currentDanger = .5f;

    public void RemoveDanger()
    {
        currentDanger--;
    }

    public void AddDanger()
    {
        currentDanger++;
        DelayUtility.ExecuteAfterSeconds(RemoveDanger, 5f);
    }
}
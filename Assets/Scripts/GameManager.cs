using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;

    public Vibe vibe;
    public Danger danger;

    public bool isGameOver;

    private float _satiatedTimer;//if more than designated value, isGameOver true
    
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
        vibe.CurrentVibeValue();

        Hunger();
    }

    private void Hunger()
    {
        if (vibe.isSatiated)
        {
            _satiatedTimer += Time.deltaTime;

            if (_satiatedTimer >= 5f)
            {
                isGameOver = true;
            }
        }
        else
        {
            _satiatedTimer = 0f;
        }
    }
}

public class Vibe
{
    public float currentValue = 0f;
    public float maxValue = 100f;
    public float reduceValue = 1f;
    public float reduceMultiplier = 1f;

    public bool isFull;
    public bool isSatiated;
    private bool _isDazeState;

    public void CurrentVibeValue()
    {
        if(currentValue > 0)
        {
            if (currentValue < maxValue || _isDazeState)
            {
                currentValue -= reduceValue * CalculatedMultiplier() * Time.deltaTime;
                isSatiated = false;
                isFull = false;
            }
            else
            {
                currentValue = maxValue;
                
                if (!_isDazeState)
                {
                    reduceMultiplier += 3.5f;
                    GameManager.Instance.player.StartDazeState();
                    void DelayDazeEnding()
                    {
                        GameManager.Instance.player.EndDazeState();
                        _isDazeState = false;
                        reduceMultiplier -= 3.5f;
                    }
                    DelayUtility.ExecuteAfterSeconds(DelayDazeEnding, 2f);
                    _isDazeState = true;
                }
            }
        }
        else
        {
            currentValue = 0;
            isSatiated = true;
        }
    }

    private float CalculatedMultiplier()
    {
        return reduceMultiplier * GameManager.Instance.player.stateVibeReduceMultiplier;
    }
}

public class Danger
{
    public float currentDanger;
    public float dangerRemoveCooldown;

    public void RemoveDanger()
    {
        currentDanger--;
    }

    //For bubble resummon
    public float DangerMultiplierForInterval()
    {
        return 1 * currentDanger;
    }

    public void AddDanger()
    {
        currentDanger++;
        DelayUtility.ExecuteAfterSeconds(RemoveDanger, 5f);
    }
}
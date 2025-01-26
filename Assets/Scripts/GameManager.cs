using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;
    public CanvasManager canvasManager;
    public AudioManager audioManager;

    public NPCController patrolledNPC;

    public Vibe vibe;
    public Danger danger;

    public bool isGameOver;

    private float _reduceVibeTimer;

    public List<NPCController> allNPCs;
    
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
        ReduceVibe();
    }

    private void ReduceVibe()
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
    }
}

[Serializable]
public class Vibe
{
    public float currentValue = 50;
    public float maxValue = 100f;
    public float reduceValue = .95f;
    public float reduceMultiplier = .45f;

    public bool isFull;
    public bool isHungry;
    private bool _isDazeState;

    public void CurrentVibeValue()
    {
        if (GameManager.Instance.isGameOver) return;
        
        if(currentValue > 0)
        {
            var _audioManager = GameManager.Instance.audioManager;
            if (currentValue < maxValue || _isDazeState)
            {
                if (!_isDazeState)
                {
                    if (currentValue <= 30)
                    {
                        _audioManager.musicState = AudioManager.MusicState.danger;
                    }
                    else 
                    {
                        _audioManager.musicState = AudioManager.MusicState.normal;
                    }
                }
                else
                {
                    _audioManager.musicState = AudioManager.MusicState.daze;
                }

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
                    reduceMultiplier += 8f;
                    GameManager.Instance.player.StartDazeState();
                    void DelayDazeEnding()
                    {
                        GameManager.Instance.player.EndDazeState();
                        _isDazeState = false;
                        reduceMultiplier -= 8f;
                    }
                    DelayUtility.ExecuteAfterSeconds(DelayDazeEnding, 4f, true);
                    _isDazeState = true;
                }
            }
        }
        else
        {
            GameManager.Instance.isGameOver = true;
            GameManager.Instance.canvasManager.StopTimer();

            if (GameManager.Instance.danger.currentDanger > 0)
            {
                GameManager.Instance.canvasManager.SetCaughtPanelActive();
                for (int i = 0; i < GameManager.Instance.allNPCs.Count; i++)
                {
                    GameManager.Instance.allNPCs[i].PlayerDied();
                }
            }
            else
            {
                GameManager.Instance.canvasManager.SetStarvationPanelActive();
            }
            Debug.Log("dead from hunger");
            //currentValue = 0;
            //GameManager.Instance.canvasManager.SetVibeFill();
            //isHungry = true;
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
    public float currentDanger = .18f;

    public void RemoveDanger()
    {
        currentDanger--;
    }

    public void AddDanger()
    {
        currentDanger++;
        DelayUtility.ExecuteAfterSeconds(RemoveDanger, 3f);
    }
}
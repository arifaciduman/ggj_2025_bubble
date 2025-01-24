using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Vibe vibe;
    public Danger danger;

    public bool isGameOver;
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
}

public class Vibe
{
    public float currentValue;
    public float maxValue;
    public float reduceValue = 1f;
    public float reduceMultiplier = 1f;

    public bool isFull;
    public bool isSatiated;
}

public class Danger
{
    public int currentDanger;
    public float dangerRemoveCooldown;

    public void RemoveDanger()
    {
        currentDanger--;
    }

    public void AddDanger()
    {
        currentDanger++;
    }
}
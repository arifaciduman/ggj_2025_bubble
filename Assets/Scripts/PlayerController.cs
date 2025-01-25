using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDazed;
    public Animator playerAC;

    public PlayerMovement playerMovement;
    
    public float stateVibeReduceMultiplier = .75f;

    public enum PlayerState
    {
        Idle,
        Walk,
        Sprint
    }
    public PlayerState state;

    private void Update()
    {
        stateVibeReduceMultiplier = SetStateMultiplier();
    }

    private float SetStateMultiplier()
    {
        return (int)state switch
        {
            1 => 1.25f,
            2 => 3.5f,
            _ => .75f
        };
    }

    public void EndDazeState()
    {
        isDazed = false;
        playerMovement.dazeSpeedMultiplier = 1f;
        playerAC.speed = 1f;
        playerAC.SetBool("isDazed", false);
    }
    
    public void StartDazeState()
    {
        isDazed = true;
        playerMovement.dazeSpeedMultiplier = 0.5f;
        playerAC.speed = 0.5f;
        playerAC.SetBool("isDazed", true);
    }
}

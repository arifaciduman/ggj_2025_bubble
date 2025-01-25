using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDazed;
    public Animator playerAC;

    public float stateVibeReduceMultiplier = 1f;

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
            1 => 3,
            2 => 5,
            _ => 1
        };
    }

    public void EndDazeState()
    {
        isDazed = false;
    }
    
    public void StartDazeState()
    {
        isDazed = true;
        //playerAC.SetBool("isDazed", true);
    }
}

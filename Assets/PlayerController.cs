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

    public void EndDazeState()
    {
        isDazed = false;
    }
    
    public void StartDazeState()
    {
        isDazed = true;
        playerAC.SetBool("isDazed", true);
    }
}

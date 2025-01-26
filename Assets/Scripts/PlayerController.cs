using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDazed;
    public Animator playerAC;

    public PlayerMovement playerMovement;
    
    public float stateVibeReduceMultiplier = .5f;

    public Rigidbody rb;

    public enum PlayerState
    {
        Idle,
        Walk,
        Sprint
    }
    public PlayerState state;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        stateVibeReduceMultiplier = SetStateMultiplier();
    }

    public void StarveAnim()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddExplosionForce( 10f, transform.position + Vector3.down, 3f);
    }

    private float SetStateMultiplier()
    {
        return (int)state switch
        {
            1 => 1.25f,
            2 => 2f,
            _ => .5f
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

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public bool isSprinting;
    
    public float drag;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.Space;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.linearDamping = drag;
        
        //readyToJump = true;
    }

    private void Update()
    {
        // ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            //GameManager.Instance.playerController.playerState = PlayerController.PlayerState.Idle;
            //GameManager.Instance.playerController.stateVibeReduceMultiplier = 1f;
        }
        else
        {
            if (Input.GetKey(sprintKey))
            {
                isSprinting = true;
                //GameManager.Instance.playerController.playerState = PlayerController.PlayerState.Sprint;
                //GameManager.Instance.playerController.stateVibeReduceMultiplier = 5f;
            }
            else
            {
                isSprinting = false;
                //GameManager.Instance.playerController.playerState = PlayerController.PlayerState.Walk;
                //GameManager.Instance.playerController.stateVibeReduceMultiplier = 3f;
            }
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (!isSprinting)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (!isSprinting)
        {
            // limit velocity if needed
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
        else
        {
            // limit velocity if needed
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * sprintSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
        
    }

}
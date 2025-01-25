using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public bool isSprinting;
    public float dazeSpeedMultiplier = 1f;
    
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
        dazeSpeedMultiplier = 1f;
        //readyToJump = true;
    }

    private void Update()
    {
        // ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (!GameManager.Instance.isGameOver)
        {
            MyInput();
            SpeedControl();
        }
        
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGameOver)
        {
            MovePlayer();
        }
        
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            RunOrSprint(PlayerController.PlayerState.Idle, false);
        }
        else
        {
            if (Input.GetKey(sprintKey))
            {
                RunOrSprint(PlayerController.PlayerState.Sprint, true);
            }
            else
            {
                RunOrSprint(PlayerController.PlayerState.Walk, false);
            }
        }
    }

    private void RunOrSprint(PlayerController.PlayerState state, bool isSprinting)
    {
        this.isSprinting = isSprinting;
        GameManager.Instance.player.state = state;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (!isSprinting)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * dazeSpeedMultiplier, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * sprintSpeed * 10f * dazeSpeedMultiplier, ForceMode.Force);
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

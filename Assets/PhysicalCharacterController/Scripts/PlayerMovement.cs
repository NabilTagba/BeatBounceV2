using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float runSpeed;
    public float groudDrag;
    public float slideSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float currentMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchYScale;
    private float StartYScale;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    private bool sliding;
    private Vector3 slideDirection;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopehit;
    private bool exitingSlope;

    [Header("WallRun")]
    public LayerMask whatIsWall;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public float wallCheckDistance;
    public float minJump;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    public float wallRunSpeed;
    private bool wallRunning;
    public float wallJumpUpForce;
    public float wallJumpSideForce;

    [Header("WallRunExit")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    [Header("Other")]
    public Transform orientation;
    public GameObject camPos;

    float horizontalInput;
    float verticalInput;
    public bool AllowMovement = true;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    PlayerControls controls;
    Gamepad gameControllerOne;
    Gamepad gameControllerTwo;

    [SerializeField] int playerIndex = 0;
    private void Awake()
    {
        controls = new PlayerControls();
        
    }
    public enum MovementState
    {
        run,
        wallrunning,
        slide,
        air
    }

    // Start is called before the first frame update
    void Start()
    {
        gameControllerOne = Gamepad.all[playerIndex];
        

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        StartYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround) && readyToJump;

        if(AllowMovement)
        {
            MyInput();
        }
        SpeedControl();
        StateHandler();
        CheckForWall();
        ChangeAirControl();

        if (grounded)
        {
            rb.drag = groudDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (wallRunning)
        {
            WallRunningMovement();
        }
        if (sliding)
        {
            SlidingMovement();
        }
    }

    private void MyInput()
    {
        horizontalInput = gameControllerOne.leftStick.x.ReadValue();
        verticalInput = gameControllerOne.leftStick.y.ReadValue();
        
        if (gameControllerOne.aButton.wasReleasedThisFrame && readyToJump && grounded && !wallRunning && !exitingWall)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
        if ((wallLeft || wallRight) && verticalInput > 0 && MinJumpForWallRun() && !exitingWall)
        {
            if (!wallRunning)
            {
                StartWallRun();
            }
            if (gameControllerOne.aButton.wasReleasedThisFrame)
            {
                WallJump();
            }

        }
        else if (exitingWall)
        {
            if (wallRunning)
            {
                StopWallRun();
            }
            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else if (wallRunning)
        {
            StopWallRun();
        }
        if (gameControllerOne.leftStickButton.wasReleasedThisFrame && (horizontalInput != 0 || verticalInput != 0))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (grounded)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
            StartSlide();
        }
        if (gameControllerOne.leftStickButton.wasReleasedThisFrame)
        {
            StopSlide();
        }
    }

    private void StateHandler()
    {
        if (wallRunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        else if (sliding)
        {
            state = MovementState.slide;

            if (OnSlope() && rb.velocity.y < .1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = runSpeed;
            }
        }
        else if (grounded)
        {
            state = MovementState.run;
            desiredMoveSpeed = runSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 3 && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            float speed = Vector3.Magnitude(rb.velocity);
            if (OnSlope() || (wallRunning && speed <= wallRunSpeed))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopehit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                if (grounded && !sliding && !wallRunning)
                {
                    time += Time.deltaTime * speedIncreaseMultiplier * 2;
                }
                else if (grounded && !wallRunning)
                {
                    time += Time.deltaTime * speedIncreaseMultiplier;
                }
                else
                {
                    if (!sliding && !wallRunning)
                    {
                        time += Time.deltaTime * speedIncreaseMultiplier * .3f;
                    }
                    /*else
                    {
                        time += Time.deltaTime * speedIncreaseMultiplier * .1f;
                    }*/

                }

            }

            //float netHorizontalSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
            if (speed < runSpeed && desiredMoveSpeed < startValue)
            {
                Debug.Log("Speed too low");
                StopCoroutine(SmoothlyLerpMoveSpeed());
                time += speedIncreaseMultiplier * Time.deltaTime * 20;
            }


            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        else if (grounded && !sliding && !wallRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded && !sliding && !wallRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        currentMoveSpeed = Vector3.Magnitude(rb.velocity);
        //Limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    private void ChangeAirControl()
    {

        if (currentMoveSpeed > 16)
        {
            airMultiplier = .05f;
        }
        else if (currentMoveSpeed > 14)
        {
            airMultiplier = .075f;
        }
        else if (currentMoveSpeed > 12)
        {
            airMultiplier = .1f;
        }
        else if (currentMoveSpeed > 10)
        {
            airMultiplier = .2f;
        }
        else
        {
            airMultiplier = .3f;
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if (sliding)
        {
            slideTimer = maxSlideTime;
        }
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        //Checks if you are on a slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopehit, playerHeight * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopehit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        //calculates the slopes angle
        return Vector3.ProjectOnPlane(direction, slopehit.normal).normalized;
    }

    private void StartSlide()
    {
        sliding = true;

        slideTimer = maxSlideTime;

        slideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }

    private void SlidingMovement()
    {
        if (!grounded)
        {
            slideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }
        if (!OnSlope() || rb.velocity.y > -.1f)
        {
            //rb.AddForce(slideDirection.normalized * slideForce, ForceMode.Force);

            if (grounded)
            {
                //rb.AddForce(moveDirection.normalized * moveSpeed * slideForce, ForceMode.Force);
                //rb.AddForce(moveDirection.normalized * slideForce, ForceMode.Force);
                Vector3 groundSlideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput * .5f;
                //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                groundSlideDirection += rb.velocity;
                rb.AddForce(groundSlideDirection.normalized * moveSpeed * 10f, ForceMode.Force);

                //rb.AddForce(slideDirection.normalized * slideForce, ForceMode.Force);
                slideTimer -= Time.deltaTime;
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }

        }
        else
        {
            //Sliding Downwards
            rb.AddForce(GetSlopeMoveDirection(slideDirection) * slideForce, ForceMode.Force);

        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
        sliding = false;
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool MinJumpForWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJump, whatIsGround);
    }

    private void StartWallRun()
    {
        wallRunning = true;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

    }

    private void StopWallRun()
    {
        wallRunning = false;
        rb.useGravity = true;

    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    public void StopPlayerMovement()
    {
        AllowMovement = false;
        Invoke("StartPlayerMovement", 3f);
    }

    public void StartPlayerMovement()
    {
        AllowMovement = true;
    }

}

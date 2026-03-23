using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 6f;
    public float sprintMultiplier = 2f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;

    [Header("Dash Settings")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.15f;

    [Header("Wall Run Settings")]
    public LayerMask wallMask;
    public float wallRunGravity = -1.5f;
    public float wallJumpForce = 12f;
    public float wallJumpSideForce = 10f;
    public float wallDetectionRange = 0.8f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isDashing = false;
    private float dashTime;
    private bool isWallRunning = false;
    private bool wallRight;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;

        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
            isWallRunning = false;
        }

        CheckWallRun();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? baseSpeed * sprintMultiplier : baseSpeed;

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }
        
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
        }

        if (isDashing)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            if (Time.time > dashTime) isDashing = false;
        }
        else
        {
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isWallRunning)
            {
                Vector3 wallNormal = wallRight ? -transform.right : transform.right;
                velocity = (Vector3.up * wallJumpForce) + (wallNormal * wallJumpSideForce);
                jumpCount = 1;
                isWallRunning = false;
            }
            else if (isGrounded || jumpCount < 2)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
            }
        }

        float currentGravity = isWallRunning ? wallRunGravity : gravity;
        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CheckWallRun()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, wallDetectionRange, wallMask);
        bool wallLeft = Physics.Raycast(transform.position, -transform.right, wallDetectionRange, wallMask);

        if ((wallLeft || wallRight) && !controller.isGrounded && Input.GetAxis("Vertical") > 0)
        {
            isWallRunning = true;
            if (velocity.y < 0) jumpCount = 1;
        }
        else 
        { 
            isWallRunning = false; 
        }
    }

    public void ResetPlayerState()
    {
        velocity = Vector3.zero;
        jumpCount = 0;
        isDashing = false;
        isWallRunning = false;
    }

    public void ApplyUpdraft(float force)
    {
        velocity.y = force;
        jumpCount = 1;
    }
}
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

    [Header("Updraft Physics")]
    public float airResistance = 2.5f; //Air Resistance (Drag Coefficient - k)

    [Header("Wall Run Settings")]
    public LayerMask wallMask;
    public float wallRunGravity = -1.5f; //Friction
    public float wallJumpForce = 12f;
    public float wallJumpSideForce = 10f;
    public float wallDetectionRange = 0.8f;

    [Header("Climbing Settings")]
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private bool canClimb = false;

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
        
        if (Input.GetKeyDown(KeyCode.E) && !isDashing) //Newton's rule no.1 (Law of Inertia) -> Dashing is using high velocity for a while
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

        if (canClimb && Input.GetAxis("Vertical") > 0)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical"); //Friction
        
            velocity.y = verticalInput * climbSpeed; 
        
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isWallRunning)
            {
                Vector3 wallNormal = wallRight ? -transform.right : transform.right; //Newton's rule no.3 (Action-Reaction) -> 
                                                                                     //used force to push away from wall in opposite direction
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

        float currentGravity = isWallRunning ? wallRunGravity : gravity; //Newton's 'rule no.2 (F=ma) -> Gravity's acceleration
        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CheckWallRun()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, wallDetectionRange, wallMask); //Unity Physics 3D (Raycast) -> Check wall
                                                                                                        //To use Collision Detection that it is Wall-Layer
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

    public void ApplyUpdraft(float windForce)
    {
        float dragForce = airResistance * velocity.y; //Simulate air resistance (Drag = k * v)

        velocity.y += (windForce - dragForce) * Time.fixedDeltaTime; //Newton's 'rule no.2 (F=ma) -> Use force to push player up
        jumpCount = 1;
    }

    private void OnTriggerEnter(Collider other) //Unity Physics 3D (Trigger)
    {
        if (other.CompareTag("Climbable")) canClimb = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            canClimb = false;
            isClimbing = false;
        }
    }
}
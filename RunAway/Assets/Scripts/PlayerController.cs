using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 6f;
    public float sprintMultiplier = 2f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isDashing = false;
    private float dashTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float currentSpeed = baseSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        
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

        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ResetPlayerState()
    {
        velocity = Vector3.zero;
        jumpCount = 0;
        isDashing = false;
    }

    public void ApplyUpdraft(float windForce)
    {
        velocity.y = windForce; 
    }
}
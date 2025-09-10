using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float minLookAngle = -60f;
    public float maxLookAngle = 60f;

    private CharacterController controller;
    //private Animator animator;

    private Vector3 velocity;
    private float xRotation = 0f;

    // Ability toggles
    public bool canWalk = true;
    public bool canSprint = true;
    public bool canJump = true;
    public bool canLook = true;
    public bool canPressButtons = true; // 👈 new toggle for left-click

    private Vector3 platformVelocity;
    private MovingPlatform currentPlatform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleJumpInput();
        //UpdateAnimator();
        HandleButtonInput(); // 👈 handle left-click disabling
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!canWalk) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = (canSprint && Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : walkSpeed;
        controller.Move(move * speed * Time.fixedDeltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);

        // Platform movement
        if (controller.isGrounded && currentPlatform != null)
        {
            controller.Move(platformVelocity * Time.deltaTime);
        }
    }

    void HandleJumpInput()
    {
        if (canJump && controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //animator.SetTrigger("JumpStart");
        }
    }

    void HandleMouseLook()
    {
        if (!canLook)
        {
            //xRotation = 0f;
            //cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //* Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; //* Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleButtonInput()
    {
        if (!canPressButtons)
        {
            // Left-click disabled: ignore Input
            if (Input.GetMouseButtonDown(0))
            {
                // Optionally: play a “disabled” sound or effect here
                return;
            }
        }
        else
        {
            // Normal left-click interaction logic can go here
            if (Input.GetMouseButtonDown(0))
            {
                // Example: interact with buttons, doors, etc.
            }
        }
    }

    public void DisableSprint() => canSprint = false;
    public void DisableJump() => canJump = false;
    public void DisableLook() => canLook = false;
    public void DisableButtonPress() => canPressButtons = false; // 👈 new method

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            currentPlatform = hit.collider.GetComponent<MovingPlatform>();
            if (currentPlatform != null)
                platformVelocity = currentPlatform.platformVelocity;
        }
        else
        {
            currentPlatform = null;
            platformVelocity = Vector3.zero;
        }
    }
}

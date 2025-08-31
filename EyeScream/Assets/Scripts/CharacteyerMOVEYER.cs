using UnityEngine;
using UnityEngine.InputSystem;

public class CharacteyerMOVEYER : MonoBehaviour
{
    public float speed;
    public float gravity;

    /// <summary>
    /// Degrees per second
    /// </summary>
    public float rotateSpeed;
    public float jumpHeight;
    public Animator animator; // your idle/walk/run animator

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isInteracting = false;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do shiz if interacting
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("eyenteract"))
        {
            return;
        }

        // Check if the character is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // reset downward velocity when grounded
        }

        // WASD / arrow input
        Vector3 move = transform.forward * moveInput.y;

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Rotation
        transform.Rotate(0f, moveInput.x * rotateSpeed * Time.deltaTime, 0f);

        // Update animator
        bool isMoving = move.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
    }

    // New input system
    public void OnMoveye(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnEyenteract(InputAction.CallbackContext context)
    {
        if (context.performed && !isInteracting) // only when button is pressed, not released
        {
            animator.SetTrigger("interact");
        }
    }
}

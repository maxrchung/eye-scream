using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacteyerMOVEYER : MonoBehaviour
{
    public float speed;
    public float gravity;

    /// <summary>
    /// Some inherent gravity so things always try to stick to the ground
    /// </summary>
    public float groundedGravity = -2f;

    /// <summary>
    /// Degrees per second
    /// </summary>
    public float rotateSpeed;
    public float jumpHeight;
    public Animator animator; // your idle/walk/run animator


    /// <summary>
    /// How long in seconds to disable character during interaction I can't be
    /// arsed to somehow figure out setting the event shit properly
    /// </summary>
    public float interactTime;

    /// <summary>
    /// Da color of da playa. Might be enummed later idk FK IT
    /// </summary>
    public string coleyer;

    public GameObject toeyerchOndaChar;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isInteracting = false;
    private Vector2 moveInput;
    private Eyenteractable currentEyenteractable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do shiz if interacting
        if (isInteracting)
        {
            return;
        }

        // Check if the character is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundedGravity; // reset downward velocity when grounded
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

    void OnTriggerEnter(Collider other)
    {
        var eyenteractable = other.GetComponent<Eyenteractable>();
        if (eyenteractable != null && eyenteractable.isEyenteractable)
        {
            currentEyenteractable = eyenteractable;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Eyenteractable>() == currentEyenteractable)
        {
            currentEyenteractable = null;
        }
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
            StartCoroutine(AnimateEyenteract());

            if (currentEyenteractable != null &&
                currentEyenteractable.isEyenteractable &&
                (currentEyenteractable.coleyer == coleyer || currentEyenteractable.coleyer == ""))
            {
                currentEyenteractable.Eyenteract(gameObject);
            }
        }
    }

    public void ShowToeyerch()
    {
        toeyerchOndaChar.SetActive(true);
    }

    private IEnumerator AnimateEyenteract()
    {
        isInteracting = true;
        animator.SetTrigger("interact");

        // I hope this picks the rigth sheyet
        yield return new WaitForSeconds(interactTime);

        isInteracting = false;
    }
}

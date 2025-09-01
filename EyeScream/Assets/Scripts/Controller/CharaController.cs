using Config;
using System;
using Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CharaController : MonoBehaviour, InputActions.IPlayerActions
    {
        public float speed = 5f;
        public PlayerColor color = PlayerColor.None;
        public GameObject toeyerchOndaChar;
        public GameObject keyOnChar;
        private Eyenteractable currentEyenteractable;


        private static readonly int Interact = Animator.StringToHash("interact");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
        private Animator _animator;
        private Vector2 _inputVector = Vector2.zero;
        private Vector3 _velocity = Vector3.zero;

        private GameObject _activeCamera;
        private CharacterController _controller;

        private Material overlay;

        private void Awake()
        {
            var renderers = GetComponentsInChildren<Renderer>();

            // Surely it's the first one
            if (renderers.Length >= 1)
            {
                var renderer = renderers[0];

                var materials = renderer.materials;
                materials[0].SetColor("emissiveFactor", color switch
                {
                    PlayerColor.Red => Color.red * 0.1f,
                    PlayerColor.Green => Color.green * 0.1f,
                    PlayerColor.Blue => Color.blue * 0.1f,
                    PlayerColor.Purple => Color.purple * 0.1f,
                    _ => Color.white
                });
                renderer.materials = materials;
            }

            var keyInteractable = keyOnChar.GetComponentInChildren<Renderer>();
            keyInteractable.materials[0]
                .SetColor("emissiveFactor", renderers[0].materials[0].GetColor("emissiveFactor"));
        }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var isGrounded = _controller.isGrounded;
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2; // reset downward velocity when grounded
            }

            // WASD / arrow input
            var move = transform.forward * _inputVector.y;
            if (!_animator.GetBool(IsInteracting))
            {
                _controller.Move(move * (speed * Time.deltaTime));
                transform.Rotate(0f, _inputVector.x * 222 * Time.deltaTime, 0f);
            }

            // Apply gravity
            _velocity.y += -9.81f * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            if (move.magnitude > 0.1f)
            {
                _animator.SetBool(IsMoving, true);
            }
            else
            {
                _animator.SetBool(IsMoving, false);
            }
        }

        public void ActivateInput(InputActions.PlayerActions actions, GameObject activeCamera)
        {
            actions.AddCallbacks(this);
            _activeCamera = activeCamera;
        }

        public void CancelInput(InputActions.PlayerActions actions)
        {
            _inputVector = Vector2.zero;
            actions.RemoveCallbacks(this);
            if (_animator != null)
                _animator.SetBool(IsMoving, false);
        }

        public bool HasToeyerch()
        {
            return toeyerchOndaChar.activeSelf;
        }

        public void EquipToeyerch()
        {
            toeyerchOndaChar.SetActive(true);
        }

        public void EquipKeyey()
        {
            keyOnChar.SetActive(true);
        }

        public void UnequipKeyey()
        {
            keyOnChar.SetActive(false);
        }

        public bool HasKeyey()
        {
            return keyOnChar.activeSelf;
        }

        void OnTriggerEnter(Collider other)
        {
            var eyenteractable = other.GetComponent<Eyenteractable>();
            if (CanEyenteract(eyenteractable))
            {
                var prompt = eyenteractable.GetComponentInChildren<InteractPrompt>();
                prompt?.Show(_activeCamera);
                currentEyenteractable = eyenteractable;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Eyenteractable>() == currentEyenteractable)
            {
                var prompt = currentEyenteractable.GetComponentInChildren<InteractPrompt>();
                prompt?.Hide();
                currentEyenteractable = null;
            }
        }

        public bool CanEyenteract(Eyenteractable what)
        {
            return what != null && what.isEyenteractable &&
                   (what.coleyer == color || what.coleyer == PlayerColor.Any);
        }

        public bool CanEyenteract() => CanEyenteract(currentEyenteractable);

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && !_animator.GetBool(IsInteracting) && CanEyenteract())
            {
                _animator.SetTrigger(Interact);
                var prompt = currentEyenteractable.GetComponentInChildren<InteractPrompt>();
                prompt?.Hide();
            }
        }

        public void OnInteractCenter()
        {
            if (CanEyenteract())
            {
                currentEyenteractable.Eyenteract(gameObject);
            }
        }

        public void OnInteractFinished()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _inputVector = context.ReadValue<Vector2>();
            }
            else
            {
                _animator.SetBool(IsMoving, false);
                _inputVector = Vector2.zero;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // No lmao
        }
    }
}
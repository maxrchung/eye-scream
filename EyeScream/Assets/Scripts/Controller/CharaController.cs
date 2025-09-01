using Config;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CharaController : MonoBehaviour, InputActions.IPlayerActions
    {
        public float speed = 5f;
        public string coleyer = "reyed";
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
                Array.Resize(ref materials, 2);

                overlay = new Material(Shader.Find("Unlit/Color"));

                if (coleyer == "reyed")
                {
                    overlay.color = Color.red;
                }
                else if (coleyer == "blueye")
                {
                    overlay.color = Color.blue;
                }
                else if (coleyer == "yeyellow")
                {
                    overlay.color = Color.yellow;
                }
                else
                {
                    overlay.color = Color.clear;
                }

                materials[1] = overlay;
                renderer.materials = materials;
            }
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
            _animator?.SetBool(IsMoving, false);
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && !_animator.GetBool(IsInteracting))
            {
                _animator.SetTrigger(Interact);
            }
        }

        public void OnInteractCenter()
        {
            if (currentEyenteractable != null &&
                currentEyenteractable.isEyenteractable &&
                (currentEyenteractable.coleyer == coleyer || currentEyenteractable.coleyer == ""))
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
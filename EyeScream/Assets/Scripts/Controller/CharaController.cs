using System;
using Config;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CharaController : MonoBehaviour, InputActions.IPlayerActions
    {
        public float speed = 5f;

        private static readonly int Interact = Animator.StringToHash("interact");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
        private Animator _animator;
        private Vector2 _inputVector = Vector2.zero;
        private Vector3 _velocity = Vector3.zero;

        private GameObject _activeCamera;
        private CharacterController _controller;

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

            // Move the character
            _controller.Move(move * (speed * Time.deltaTime));

            // Apply gravity
            _velocity.y += -9.81f * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            // Rotation
            transform.Rotate(0f, _inputVector.x * 222 * Time.deltaTime, 0f);

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
            _animator.SetBool(IsMoving, false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && !_animator.GetBool(IsInteracting))
                _animator.SetTrigger(Interact);
        }

        public void OnInteractCenter()
        {
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
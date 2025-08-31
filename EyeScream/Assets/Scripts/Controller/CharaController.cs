using System;
using Config;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CharaController : MonoBehaviour, InputActions.IPlayerActions
    {
        private static readonly int Interact = Animator.StringToHash("interact");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
        }

        public void ActivateInput(InputActions.PlayerActions actions)
        {
            actions.AddCallbacks(this);
        }

        public void CancelInput(InputActions.PlayerActions actions)
        {
            actions.RemoveCallbacks(this);
            _animator.SetBool(IsMoving, false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                _animator.SetTrigger(Interact);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
                _animator.SetBool(IsMoving, true);
            else if (context.canceled)
                _animator.SetBool(IsMoving, false);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // No lmao
        }
    }
}
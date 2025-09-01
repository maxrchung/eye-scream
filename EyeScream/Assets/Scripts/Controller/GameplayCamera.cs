using System;
using System.Collections.Generic;
using Config;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Controller
{
    public enum CameraPlayerNumber
    {
        NoPlayer,
        RedPlayer,
        GreenPlayer,
        BluePlayer,
        PurplePlayer,
    }

    public class GameplayCamera : MonoBehaviour, InputActions.IPlayerActions
    {
        public CameraPlayerNumber playerNumber = CameraPlayerNumber.NoPlayer;
        public CharaController character;

        private float _pitch;
        private float _yaw;

        private bool _wasGamepad = false;
        private Vector2 _gamepadVector = Vector2.zero;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
        }

        private void Update()
        {
            if (_wasGamepad)
            {
                OnCameraMove(_gamepadVector * Time.deltaTime);
            }
        }

        public void ActivateInput(InputActions.PlayerActions actions)
        {
            actions.AddCallbacks(this);
            character?.ActivateInput(actions, gameObject);
        }

        public void CancelInput(InputActions.PlayerActions actions)
        {
            character?.CancelInput(actions);
            actions.RemoveCallbacks(this);
            _wasGamepad = false;
        }

        public void OnCameraMove(Vector2 input)
        {
            _pitch -= input.y;
            _yaw += input.x;
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            // Yea we don't do that here
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.control.device is Gamepad)
            {
                _wasGamepad = true;
                _gamepadVector = context.ReadValue<Vector2>();
                return;
            }

            _wasGamepad = false;
            _gamepadVector = Vector2.zero;

            OnCameraMove(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }
    }
}
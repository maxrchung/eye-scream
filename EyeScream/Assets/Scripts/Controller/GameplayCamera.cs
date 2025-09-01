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

    public class GameplayCamera : MonoBehaviour, InputActions.ICameraActions
    {
        public CameraPlayerNumber playerNumber = CameraPlayerNumber.NoPlayer;
        public CharaController character;

        private float _pitch;
        private float _yaw;

        private bool _wasGamepad = false;
        private Vector2 _gamepadVector = Vector2.zero;
        private Camera cam;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_wasGamepad)
            {
                OnCameraMove(_gamepadVector * Time.deltaTime);
            }
        }

        public void ActivateInput(InputActions actions)
        {
            actions.Camera.AddCallbacks(this);
            character?.ActivateInput(actions.Player, gameObject);
        }

        public void CancelInput(InputActions actions)
        {
            character?.CancelInput(actions.Player);
            actions.Camera.RemoveCallbacks(this);
            _wasGamepad = false;
        }

        public void OnCameraMove(Vector2 input)
        {
            var zoomFactor = cam.fieldOfView / 60.0f;
            _pitch -= input.y * zoomFactor;
            _yaw += input.x * zoomFactor;
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
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

        public void OnZoom(InputAction.CallbackContext context)
        {
            var zoom = context.ReadValue<float>();
            if (Math.Abs(zoom) > 0.01f)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - zoom, 10.0f, 60.0f);
            }
        }
    }
}
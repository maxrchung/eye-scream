using System;
using System.Collections.Generic;
using Config;
using Interfaces;
using Types;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Controller
{
    public class GameplayCamera : MonoBehaviour, InputActions.ICameraActions
    {
        public CharaController character;

        private float _pitch;
        private float _yaw;

        private bool _wasGamepad = false;
        private Vector2 _gamepadVector = Vector2.zero;
        private Camera cam;
        private Color _fogColor;
        public Color FogColor => _fogColor;
        private Color _originalFogColor;
        private bool _originalFogEnabled;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
            cam = GetComponent<Camera>();

            if (character != null)
            {
                _fogColor = character.color switch
                {
                    PlayerColor.Red => Color.indianRed * 0.5f,
                    PlayerColor.Green => Color.lightGreen * 0.5f,
                    PlayerColor.Blue => Color.skyBlue * 0.5f,
                    PlayerColor.Purple => Color.mediumPurple * 0.5f,
                    _ => Color.gray3
                };
            }
            else
            {
                _fogColor = Color.gray3;
            }

            _originalFogColor = RenderSettings.fogColor;
            _originalFogEnabled = RenderSettings.fog;
        }

        private void Update()
        {
            if (_wasGamepad)
            {
                OnCameraMove(_gamepadVector * Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private void OnBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
        {
            // Only apply fog color if this is our camera
            if (renderingCamera == cam)
            {
                RenderSettings.fogColor = _fogColor;
                RenderSettings.fog = true; // Ensure fog is enabled
            }
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
        {
            // Restore original fog settings after our camera is done
            if (renderingCamera == cam)
            {
                RenderSettings.fogColor = _originalFogColor;
                RenderSettings.fog = _originalFogEnabled;
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
            _pitch = Mathf.Clamp(_pitch, 25f, 60f);
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
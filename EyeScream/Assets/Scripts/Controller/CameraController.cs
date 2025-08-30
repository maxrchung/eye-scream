using Config;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        public float mouseSensitivity = 80f;
        public float gamepadSensitivity = 50f;

        private float _pitch = 0.0f;
        private float _yaw = 0.0f;
        private InputActions _inputActions;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
        }

        private void RotateCamera(Vector2 delta)
        {
            _pitch -= delta.y;
            _yaw += delta.x;
            _pitch = Mathf.Clamp(_pitch, -89f, 89f);
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }

        private void Update()
        {
            if (_inputActions.Player.LookIndirectTrigger.IsPressed())
            {
                var updateDelta = _inputActions.Player.LookIndirect.ReadValue<Vector2>() *
                                  (0.01f * mouseSensitivity * -1f);
                RotateCamera(updateDelta);
            }
            else if (_inputActions.Player.LookDirect.IsInProgress())
            {
                var updateDelta = _inputActions.Player.LookDirect.ReadValue<Vector2>() *
                                  (Time.deltaTime * gamepadSensitivity);
                RotateCamera(updateDelta);
            }
        }
    }
}
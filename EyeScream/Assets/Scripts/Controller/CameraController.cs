using System.Collections.Generic;
using Config;
using UnityEngine;
using UnityEngine.Rendering;

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

    class PlayerMetadata
    {
        public static readonly Dictionary<CameraPlayerNumber, Color> PlayerColors =
            new()
            {
                { CameraPlayerNumber.RedPlayer, Color.red },
                { CameraPlayerNumber.GreenPlayer, Color.green },
                { CameraPlayerNumber.BluePlayer, Color.blue },
                { CameraPlayerNumber.PurplePlayer, new Color(0.5f, 0f, 0.5f) }
            };
    }

    public class CameraController : MonoBehaviour
    {
        public float mouseSensitivity = 80f;
        public float gamepadSensitivity = 50f;
        public Vector2 pitchMinMax = new(-40, 85);
        public Vector2 yawMinMax = new(-90, 90);
        public CameraPlayerNumber playerNumber = CameraPlayerNumber.NoPlayer;
        public Volume volume;

        private float _pitch;
        private float _yaw;
        private InputActions _inputActions;
        private Camera _camera;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
            _camera = GetComponent<Camera>();
        }

        private void RotateCamera(Vector2 delta)
        {
            _pitch -= delta.y;
            _yaw += delta.x;
            _pitch = Mathf.Clamp(_pitch, pitchMinMax.x, pitchMinMax.y);
            _yaw = Mathf.Clamp(_yaw, yawMinMax.x, yawMinMax.y);
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
using System.Collections.Generic;
using Config;
using UnityEngine;
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

    public class GameplayCamera : MonoBehaviour
    {
        public CameraPlayerNumber playerNumber = CameraPlayerNumber.NoPlayer;

        private float _pitch;
        private float _yaw;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
        }

        public void RotateCamera(Vector2 delta)
        {
            _pitch -= delta.y;
            _yaw += delta.x;
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }
    }
}
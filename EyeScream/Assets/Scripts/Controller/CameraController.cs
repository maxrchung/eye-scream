using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;

    private float _pitch = 0.0f;
    private float _yaw = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pitch = transform.eulerAngles.x;
        _yaw = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            var delta = Mouse.current.delta.ReadValue();
            _yaw -= sensitivity * delta.x;
            _pitch += sensitivity * delta.y;
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }
    }
}
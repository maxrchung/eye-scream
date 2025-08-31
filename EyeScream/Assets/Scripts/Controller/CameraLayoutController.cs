using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Controller
{
    public class CameraLayoutController : MonoBehaviour
    {
        private struct SavedCamera
        {
            public Camera Cam;
            public GameplayCamera Ctl;

            public SavedCamera(Camera cam)
            {
                Cam = cam;
                Ctl = cam.GetComponent<GameplayCamera>();
            }
        }

        public float mouseSensitivity = 15f;
        public float gamepadSensitivity = 50f;

        private List<SavedCamera> _cameras = new();
        private SavedCamera? _activeCamera = null;

        private bool InSingleMode => _activeCamera != null;
        private InputActions _inputActions;
        private InputActions.UIActions UIActions => _inputActions.UI;
        private InputActions.PlayerActions PlayerActions => _inputActions.Player;
        private RenderUtils _ru;

        [Header("Border Colors")] //
        public float overlayOpacity = 0.15f;

        public Color defaultBorderColor = Color.white;

        public Color borderColorPlayerRed = Color.red;
        public Color borderColorPlayerGreen = Color.green;
        public Color borderColorPlayerBlue = Color.blue;
        public Color borderColorPlayerPurple = new(0.5f, 0f, 0.5f);

        public Color GetPlayerColor(CameraPlayerNumber playerNumber)
        {
            switch (playerNumber)
            {
                case CameraPlayerNumber.RedPlayer:
                    return borderColorPlayerRed;
                case CameraPlayerNumber.GreenPlayer:
                    return borderColorPlayerGreen;
                case CameraPlayerNumber.BluePlayer:
                    return borderColorPlayerBlue;
                case CameraPlayerNumber.PurplePlayer:
                    return borderColorPlayerPurple;
                case CameraPlayerNumber.NoPlayer:
                default:
                    return defaultBorderColor;
            }
        }

        private void Start()
        {
            _cameras = FindCameras();
            _inputActions = new InputActions();
            _inputActions.Enable();
            _ru = new RenderUtils();
            LayoutAllGrid();
        }

        private void Update()
        {
            UpdateSingleCamera();
        }

        private void UpdateSingleCamera()
        {
            if (!_activeCamera.HasValue) return;
            if (PlayerActions.LookIndirect.IsPressed())
            {
                _activeCamera.Value.Ctl.RotateCamera(
                    PlayerActions.LookIndirect.ReadValue<Vector2>()
                    * (0.01f * mouseSensitivity * -1f)
                );
            }
            else if (PlayerActions.LookDirect.IsInProgress())
            {
                _activeCamera.Value.Ctl.RotateCamera(
                    PlayerActions.LookDirect.ReadValue<Vector2>()
                    * (Time.deltaTime * gamepadSensitivity)
                );
            }
        }

        private static List<SavedCamera> FindCameras()
        {
            return FindObjectsByType<Camera>(FindObjectsSortMode.None)
                .Where(x => x.TryGetComponent(out GameplayCamera _))
                .OrderBy(x => x.name)
                .Select(x => new SavedCamera(x)).ToList();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Camera Tools/Set Grid Layout", false)]
        private static void LayoutGridEditor()
        {
            EditorLayoutGrid(FindCameras());
        }

        [MenuItem("GameObject/Camera Tools/Set Single Layout", false)]
        private static void LayoutSingleEditor(MenuCommand menuCommand)
        {
            EditorLayoutSingle(FindCameras(), Selection.activeGameObject.GetComponent<Camera>());
        }
#endif

        private void LayoutSingle(Camera cam)
        {
            _activeCamera = new SavedCamera(cam);
            EditorLayoutSingle(_cameras, cam);
        }

        private void LayoutAllGrid()
        {
            _activeCamera = null;
            EditorLayoutGrid(_cameras);
        }


        private static void EditorLayoutSingle(List<SavedCamera> cameras, Camera camera)
        {
            foreach (var cam in cameras)
            {
                cam.Cam.enabled = false;
            }

            camera.enabled = true;
            camera.rect = new Rect(0, 0, 1, 1);
        }

        private static void EditorLayoutGrid(List<SavedCamera> cameras)
        {
            foreach (var cam in cameras)
            {
                cam.Cam.enabled = true;
            }

            var cameraCount = cameras.Count;
            var columnCount = Mathf.CeilToInt(Mathf.Sqrt(cameraCount));
            var cameraWidth = 1f / columnCount;

            for (var i = 0; i < cameraCount; i++)
            {
                var col = i % columnCount;
                var row = i / columnCount;
                var x = col * cameraWidth;
                var y = 1f - (row + 1) * cameraWidth;
                cameras[i].Cam.rect = new Rect(x, y, cameraWidth, cameraWidth);
            }
        }

        private void OnGUI()
        {
            if (InSingleMode) return;
            foreach (var cam in _cameras)
            {
                var color = GetPlayerColor(cam.Ctl.playerNumber);
                var screenRect = _ru.NormToScreen(cam.Cam.rect);
                color.a = overlayOpacity;
                _ru.DrawRect(screenRect, color);
                _ru.DrawRectOutline(
                    _ru.NormToScreen(cam.Cam.rect),
                    Color.darkGray,
                    _ru.PercentToPixels(1));
            }
        }
    }
}
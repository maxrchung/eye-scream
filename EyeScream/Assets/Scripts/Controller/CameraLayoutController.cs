using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controller
{
    public class CameraLayoutController : MonoBehaviour, InputActions.IUIActions
    {
        private class SavedCamera
        {
            public Camera Cam;
            public GameplayCamera Ctl;
            public Vector2Int Position;

            public SavedCamera(Camera cam)
            {
                Cam = cam;
                Ctl = cam.GetComponent<GameplayCamera>();
            }
        }

        public float mouseSensitivity = 15f;
        public float gamepadSensitivity = 50f;

        private List<SavedCamera> _cameras = new();
        [CanBeNull] private SavedCamera _activeCamera;
        private int _rowCount = 0;
        private Vector2Int _selectedCamera = Vector2Int.zero;

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
            _rowCount = Mathf.CeilToInt(Mathf.Sqrt(_cameras.Count));
            _inputActions = new InputActions();
            _inputActions.UI.AddCallbacks(this);
            _inputActions.Enable();
            _ru = new RenderUtils();
            LayoutAllGrid();
        }

        private void OnDestroy()
        {
            if (_activeCamera is not null)
            {
                PlayerActions.RemoveCallbacks(_activeCamera.Ctl);
                _activeCamera = null;
            }

            _inputActions.UI.RemoveCallbacks(this);
            _inputActions.Disable();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (InSingleMode) return;
            var nav = context.ReadValue<Vector2>();
            if (nav == Vector2.zero) return;
            _selectedCamera += new Vector2Int(-(int)nav.y, (int)nav.x);
            _selectedCamera.Clamp(Vector2Int.zero, new Vector2Int(_rowCount - 1, _rowCount - 1));
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            var pos = context.ReadValue<Vector2>();
            var posNorm = new Vector2(pos.x / Screen.width, pos.y / Screen.height);
            var hoveredCamera = _cameras.FirstOrDefault(x => x.Cam.rect.Contains(posNorm));
            if (hoveredCamera is null) return;
            _selectedCamera = hoveredCamera.Position;
        }

        void InputActions.IUIActions.OnClick(InputAction.CallbackContext context)
        {
            OnClick(context);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        void InputActions.IUIActions.OnBack(InputAction.CallbackContext context)
        {
            OnBack(context);
        }

        private void OnClick(InputAction.CallbackContext ctx)
        {
            if (InSingleMode) return;
            var pos = UIActions.Point.ReadValue<Vector2>();
            var normPos = new Vector2(pos.x / Screen.width, pos.y / Screen.height);
            var clickedCamera = _cameras.FirstOrDefault(cam => cam.Cam.rect.Contains(normPos));
            if (clickedCamera is not null)
            {
                LayoutSingle(clickedCamera.Cam);
            }
        }

        private void OnBack(InputAction.CallbackContext ctx)
        {
            if (!InSingleMode) return;
            LayoutAllGrid();
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
            _activeCamera.Ctl.ActivateInput(PlayerActions);
            EditorLayoutSingle(_cameras, cam);
        }

        private void LayoutAllGrid()
        {
            if (_activeCamera is not null)
            {
                _activeCamera.Ctl.CancelInput(PlayerActions);
                _activeCamera = null;
            }

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
                var savedCamera = cameras[i];
                savedCamera.Cam.rect = new Rect(x, y, cameraWidth, cameraWidth);
                savedCamera.Position = new Vector2Int(row, col);
            }
        }

        private void OnGUI()
        {
            if (InSingleMode) return;
            foreach (var cam in _cameras)
            {
                var color = GetPlayerColor(cam.Ctl.playerNumber);
                var screenRect = _ru.NormToScreen(cam.Cam.rect);
                //_ru.DrawRect(screenRect, color);
                color.a = overlayOpacity;
                var borderColor = Color.gray1;
                if (cam.Position == _selectedCamera)
                    borderColor = Color.yellow;
                _ru.DrawRectOutline(
                    screenRect,
                    borderColor,
                    _ru.PercentToPixels(2));
            }
        }
    }
}
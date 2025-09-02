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
    public class CameraLayoutController : MonoBehaviour, InputActions.IOverseerActions
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
        [CanBeNull] private SavedCamera _previousCamera;
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

        public GameObject backgroundCamera;
        private void Start()
        {
            backgroundCamera = GameObject.Find("BackgroundCamera");
            _cameras = FindCameras();
            _rowCount = Mathf.CeilToInt(Mathf.Sqrt(_cameras.Count));
            _inputActions = new InputActions();
            _inputActions.Overseer.AddCallbacks(this);
            _inputActions.Enable();
            _ru = new RenderUtils();
            LayoutAllGrid();
        }

        private void OnDestroy()
        {
            if (_activeCamera is not null)
            {
                _activeCamera.Ctl.CancelInput(_inputActions);
                _activeCamera = null;
            }

            _inputActions.Overseer.RemoveCallbacks(this);
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

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (InSingleMode) return;
            var selectedCam = _cameras.FirstOrDefault(cam => cam.Position == _selectedCamera);
            if (selectedCam is null)
            {
                Debug.LogWarning($"No camera found at position {_selectedCamera}");
                return;
            }

            LayoutSingle(selectedCam);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnBack(InputAction.CallbackContext ctx)
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

        private void LayoutSingle(SavedCamera cam)
        {
            _activeCamera = cam;
            backgroundCamera.GetComponent<AudioListener>().enabled = false;
            _activeCamera.Cam.gameObject.GetComponent<AudioListener>().enabled = true;
            _activeCamera!.Ctl.ActivateInput(_inputActions);
            EditorLayoutSingle(_cameras, cam.Cam);
        }

        private void LayoutAllGrid()
        {
            if (_activeCamera is not null)
            {
                _activeCamera.Ctl.CancelInput(_inputActions);
                _activeCamera = null;
            }

            EditorLayoutGrid(_cameras);
            backgroundCamera.GetComponent<AudioListener>().enabled = true;
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
                cam.Cam.gameObject.GetComponent<AudioListener>().enabled = false;
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
                var screenRect = _ru.NormToScreen(cam.Cam.rect);
                var borderColor = Color.gray1;
                if (cam.Position == _selectedCamera)
                    borderColor = Color.white;
                _ru.DrawRectOutline(
                    screenRect,
                    borderColor,
                    _ru.PercentToPixels(4));
            }
        }
    }
}
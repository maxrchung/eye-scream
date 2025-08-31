using System.Collections.Generic;
using System.Linq;
using Config;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Controller
{
    public class CameraLayoutController : MonoBehaviour
    {
        private List<Camera> _cameras = new();
        [CanBeNull] private Camera _activeCamera = null;

        private bool InSingleMode => _activeCamera != null;
        private InputActions _inputActions;
        private InputActions.UIActions _uiActions => _inputActions.UI;
        private InputActions.PlayerActions _playerActions => _inputActions.Player;

        public float mouseSensitivity = 80f;
        public float gamepadSensitivity = 50f;

        void Start()
        {
            _cameras = FindCameras();
            _inputActions = new InputActions();
            _inputActions.Enable();
            LayoutAllGrid();
        }

        void Update()
        {
            if (InSingleMode)
            {
                
            }
        }

        private static List<Camera> FindCameras()
        {
            return FindObjectsByType<Camera>(FindObjectsSortMode.None)
                .Where(x => x.TryGetComponent(out CameraController _))
                .OrderBy(x => x.name).ToList();
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
            _activeCamera = cam;
            EditorLayoutSingle(_cameras, cam);
        }

        private void LayoutAllGrid()
        {
            _activeCamera = null;
            EditorLayoutGrid(_cameras);
        }


        private static void EditorLayoutSingle(List<Camera> cameras, Camera camera)
        {
            foreach (var cam in cameras)
            {
                cam.enabled = false;
                cam.GetComponent<CameraController>().enabled = false;
            }

            camera.enabled = true;
            camera.GetComponent<CameraController>().enabled = true;
            camera.rect = new Rect(0, 0, 1, 1);
        }

        private static void EditorLayoutGrid(List<Camera> cameras)
        {
            foreach (var cam in cameras)
            {
                cam.enabled = true;
                cam.GetComponent<CameraController>().enabled = false;
            }

            var cameraCount = cameras.Count;
            var columnCount = Mathf.CeilToInt(Mathf.Sqrt(cameraCount));
            var rowCount = Mathf.CeilToInt((float)cameraCount / columnCount);
            var cameraWidth = 1f / columnCount;
            var cameraHeight = 1f / rowCount;

            for (var i = 0; i < cameraCount; i++)
            {
                var col = i % columnCount;
                var row = i / columnCount;
                var x = col * cameraWidth;
                var y = 1f - (row + 1) * cameraHeight;
                cameras[i].rect = new Rect(x, y, cameraWidth, cameraHeight);
            }
        }
    }
}
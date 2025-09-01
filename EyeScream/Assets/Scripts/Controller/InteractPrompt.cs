using System;
using UnityEngine;

namespace Controller
{
    public class InteractPrompt : MonoBehaviour
    {
        private Renderer _childRenderer;
        private GameObject _parent;

        private void Start()
        {
            _childRenderer = gameObject.GetComponentInChildren<Renderer>();
            _childRenderer.enabled = false;
        }

        private void Update()
        {
            if (_parent is null)
            {
                return;
            }

            transform.LookAt(_parent.transform);
        }

        public void Show(GameObject parent)
        {
            _parent = parent;
            _childRenderer.enabled = true;
        }

        public void Hide()
        {
            _parent = null;
            _childRenderer.enabled = false;
        }
    }
}
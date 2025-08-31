using UnityEngine;

namespace Interfaces
{
    public interface IControllable
    {
        public void OnCameraMove(Vector2 input)
        {
        }

        public void OnMove(Vector2 input, float delta)
        {
        }

        public void OnInteract()
        {
        }
    }
}
using Interfaces;
using UnityEngine;

namespace Controller
{
    public class CharaController : MonoBehaviour, IControllable
    {
        public void OnMove(Vector2 input, float delta)
        {
        }

        public void OnInteract()
        {
        }
    }
}
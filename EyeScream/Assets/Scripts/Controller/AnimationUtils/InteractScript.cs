using System;
using UnityEngine;
using UnityEngine.Animations;

namespace Controller.AnimationUtils
{
    public class InteractScript : StateMachineBehaviour
    {
        private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
        private bool _midTriggered = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsInteracting, true);
            _midTriggered = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsInteracting, false);
            animator.SendMessageUpwards("OnInteractFinished", SendMessageOptions.DontRequireReceiver);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (_midTriggered) return;
            var t = stateInfo.normalizedTime % 1f;
            if (t >= 0.5f)
            {
                _midTriggered = true;
                animator.SendMessageUpwards("OnInteractCenter", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}

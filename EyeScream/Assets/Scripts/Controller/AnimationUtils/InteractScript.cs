using UnityEngine;

namespace Controller.AnimationUtils
{
    public class InteractScript : StateMachineBehaviour
    {
        private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsInteracting, true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsInteracting, false);
        }
    }
}

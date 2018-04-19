using UnityEngine;

namespace Character2D
{
    public class RollBonus : StateMachineBehaviour
    {
        private PlayerMovement playerMovement;
        private FMOD.Studio.EventInstance rollNoise;

        private void OnEnable()
        {
            rollNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Knight/roll");
            rollNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement = animator.GetComponentInParent<PlayerMovement>();
            playerMovement.MoveBonus(1.5f);
            playerMovement.isRolling = true;

            rollNoise.start();
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement.isRolling = false;
        }
    }
}

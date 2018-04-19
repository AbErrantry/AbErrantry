using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class SlideBonus : StateMachineBehaviour
    {
        private PlayerMovement playerMovement;
        private FMOD.Studio.EventInstance slideNoise;

        private void OnEnable()
        {
            slideNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Knight/slide");
            slideNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement = animator.GetComponentInParent<PlayerMovement>();
            playerMovement.MoveBonus(1.0f);
            playerMovement.isSliding = true;

            slideNoise.start();
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement.isSliding = false;
        }
    }
}

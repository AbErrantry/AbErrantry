using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using TMPro;

namespace Character2D
{
    public class CharacterState : MonoBehaviour
    {
        public TMP_Text interactType;
        public TMP_Text interactButton;
        public GameObject interactBox;
        private float health;

        InteractCheck interactCheck;

        // Use this for initialization
        void Start()
        {
            interactCheck = GameObject.Find("Knight/TriggerBoxes/InteractTrigger").GetComponent<InteractCheck>();
            interactBox.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                if (CrossPlatformInputManager.GetButtonDown("Fire2"))
                {
                    interactCheck.InteractPress();
                }
            }
        }
    }
}

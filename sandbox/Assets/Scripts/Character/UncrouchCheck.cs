using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class UncrouchCheck : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().canUncrouch = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().canUncrouch = false;
            }
        }
    }
}

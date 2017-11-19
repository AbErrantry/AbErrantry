using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class GroundedCheck : MonoBehaviour
    {

        //used for initialization
        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().isGrounded = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().isGrounded = false;
            }
        }
    }
}

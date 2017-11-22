using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class GroundedCheck : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentGround;
        //used for initialization
        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().isGrounded = true;
                currentGround.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                currentGround.Remove(other.gameObject);
                if(currentGround.Count == 0)
                {
                    GameObject.Find("Knight").GetComponent<CharacterMovement>().isGrounded = false;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class UncrouchCheck : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentObstacles;
        //used for initialization
        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "World")
            {
                GameObject.Find("Knight").GetComponent<CharacterMovement>().canUncrouch = false;
                currentObstacles.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player")
            {
                currentObstacles.Remove(other.gameObject);
                if (currentObstacles.Count == 0)
                {
                    GameObject.Find("Knight").GetComponent<CharacterMovement>().canUncrouch = true;
                }
            }
        }
    }
}

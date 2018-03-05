using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillY : MonoBehaviour
{
    public float yLoc;

    private void Start()
    {
        transform.position = new Vector3(0.0f, yLoc, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Character2D.Attackable>() != null)
        {
            other.gameObject.GetComponent<Character2D.Attackable>().Kill();
        }
        else
        {
            Debug.Log("destroyed " + other.name.ToString());
            Destroy(other.gameObject);
        }
    }
}

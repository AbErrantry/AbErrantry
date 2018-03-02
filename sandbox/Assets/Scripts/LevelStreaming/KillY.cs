using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillY : MonoBehaviour
{
    public float yLoc;
    public Character2D.Player player;

    private void Start()
    {
        transform.position = new Vector3(0.0f, yLoc, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.tag == "Player")
        {
            player.Kill();
        }
        else
        {
            Debug.Log("destroyed " + other.name.ToString());
            Destroy(other.gameObject);
        }
    }
}

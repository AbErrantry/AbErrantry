using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelLoadTrigger : MonoBehaviour
{
    public LevelStreamManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Knight")
        {
            manager.LoadScene();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnloadTrigger : MonoBehaviour
{
    public LevelStreamManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Knight")
        {
            manager.UnloadScene();
        }
    }
}

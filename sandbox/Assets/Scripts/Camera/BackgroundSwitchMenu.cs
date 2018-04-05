using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitchMenu : MonoBehaviour
{
    public static BackgroundSwitchMenu instance; // Singleton

    [System.Serializable]
    public class Backgrounds
    {
        public GameObject grouping;
        public MeshRenderer[] backImages;
        public float[] speed;
    }

    public ScrollingBackgroundMenu scroll;
    public Backgrounds[] background;
    public int newSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        UpdateScrolling(1); //TODO: Need to change the 1 to the level they are on.
    }

    public void UpdateScrolling(int element) //element aka level
    {
        element--; //makes it work for the array, going to change this
        newSize = background[element].backImages.Length;
        scroll.ScrollChange(background[element].grouping, newSize, background[element].backImages, background[element].speed);
    }
}

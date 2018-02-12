using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Camera cam;
    public MeshRenderer[] Backgrounds; //The meshes that hold the images

    public float[] BackgroundSpeed; //The speeds of each image in the grouping
    public float speed;

    private Vector2[] offsets; //material offsets
    private Vector3 currPos; //Curr Camera pos
    private Vector3 prevPos; //prev camera pos

    private GameObject currGrouping; //Current grouping of backgrounds
    private GameObject prevGrouping; //Previous grouping of backgrounds

    // Use this for initialization
    void Start()
    {
        speed = 0;
        currPos = cam.transform.position;
        offsets = new Vector2[Backgrounds.Length];
        UpdateBackground();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        prevPos = currPos;
        currPos = cam.transform.position;
        if (cam.velocity.x != 0 && (currPos != prevPos))
        {
            UpdateBackground();
        }
    }

    public void UpdateBackground()
    {
        for (int i = 0; i < Backgrounds.Length; i++)
        {
            speed = cam.velocity.x / BackgroundSpeed[i];
            offsets[i] += new Vector2(Time.deltaTime * speed, 0);
            Backgrounds[i].material.mainTextureOffset = offsets[i];
        }
    }

    public void ScrollChange(GameObject grouping, int newSize, MeshRenderer[] newImages, float[] newSpeeds)
    {
        prevGrouping = currGrouping;
        currGrouping = grouping;

        if(prevGrouping != null)
        {
        prevGrouping.SetActive(false);
        }
        currGrouping.SetActive(true); //new background is running
        Backgrounds = new MeshRenderer[newSize]; //reinitializes the items
        BackgroundSpeed = new float[newSize];
        offsets = new Vector2[newSize];
        for (int i = 0; i < newSize; i++)//sets each image
        {
            Backgrounds[i] = newImages[i];
            BackgroundSpeed[i] = newSpeeds[i];
        }
    }
}

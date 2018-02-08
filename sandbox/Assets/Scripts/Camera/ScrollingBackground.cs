using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    public float speed = 0;
    public Camera cam;

    public MeshRenderer[] Backgrounds;
    public float[] BackgroundSpeed;
    private Vector2[] offsets;

    private Vector3 currPos;
    private Vector3 prevPos;

    // Use this for initialization
    void Start()
    {
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
        for(int i = 0; i < Backgrounds.Length; i++)
                {
                    speed = cam.velocity.x / BackgroundSpeed[i];
                    offsets[i] += new Vector2(Time.deltaTime * speed, 0);
                    Backgrounds[i].material.mainTextureOffset = offsets[i];
                    
                }
    }

    public void ScrollChange(int newSize, MeshRenderer[] newImages, float[] newSpeeds)
    {
        Backgrounds = new MeshRenderer[newSize];
        BackgroundSpeed = new float[newSize];
        offsets = new Vector2[newSize];

        for(int i=0; i <newSize; i++)
        {
            Backgrounds[i] = newImages[i];
            BackgroundSpeed[i] = newSpeeds[i];
        }
    }
}


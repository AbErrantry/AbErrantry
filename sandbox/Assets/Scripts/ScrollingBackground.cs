using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    public float speed = 0;
    public CharacterMovement player;

    public MeshRenderer[] Backgrounds;
    public float[] BackgroundSpeed;
    private Vector2[] offsets;

    private Vector3 currPos;
    private Vector3 prevPos;
    // Use this for initialization
    void Start()
    {
        currPos = player.transform.position;
        offsets = new Vector2[Backgrounds.Length];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        prevPos = currPos;
        currPos = player.transform.position;


        if (player.mvmtSpeed != 0 && (currPos != prevPos))
        {
            for(int i = 0; i < Backgrounds.Length; i++)
            {
                speed = player.mvmtSpeed / BackgroundSpeed[i];
                offsets[i] += new Vector2(Time.deltaTime * speed, 0);
                Backgrounds[i].material.mainTextureOffset = offsets[i];
            }
        }
    }
}


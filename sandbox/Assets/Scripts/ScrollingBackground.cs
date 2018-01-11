using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    public float speed = 0;
    public CharacterMovement player;
    private Vector2 mainOffset;
    private Vector2 midOffset;
    public MeshRenderer mainBackground;
    public MeshRenderer middleBackground;

    private Vector3 currPos;
    private Vector3 prevPos;
    // Use this for initialization
    void Start()
    {
        currPos = player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        prevPos = currPos;
        currPos = player.transform.position;


        if (player.mvmtSpeed != 0 && (currPos != prevPos))
        {
            speed = player.mvmtSpeed / 100;
            mainOffset += new Vector2(Time.deltaTime * speed, 0);
            mainBackground.material.mainTextureOffset = mainOffset;

            speed = player.mvmtSpeed / 50;
            midOffset += new Vector2(Time.deltaTime * speed, 0);
            middleBackground.material.mainTextureOffset = midOffset;
        }
    }
}


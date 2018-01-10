using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public float speed = 0;
    public CharacterMovement player;
    private Vector2 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        speed = player.mvmtSpeed / 4;
        if (speed == 0)
            speed = 1;
        offset = new Vector2(Time.deltaTime * speed, 0);

        this.GetComponent<MeshRenderer>().material.mainTextureOffset = offset;
	}
}

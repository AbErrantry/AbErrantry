using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParallax : MonoBehaviour {

[Tooltip("No need to set this, it is done inside the script. Viewing is for debugging purposes.")]
	public float speed;
	private Vector2 offset;

	private MeshRenderer water;
	private BuoyancyEffector2D buoy;

	// Use this for initialization
	void Start () 
	{
		speed = 0;
		water = this.gameObject.GetComponent<MeshRenderer>();
		buoy = this.gameObject.GetComponent<BuoyancyEffector2D>();
	}
	
	// Update is called once per frame
	void Update () {

		speed = (buoy.flowMagnitude/500) * -1; //Multiplied by -1 so it goes the opposite way of the magnitude, giving the illusion of it moving together.
        offset += new Vector2(Time.deltaTime * speed, 0);
        water.material.mainTextureOffset = offset;
	}
}

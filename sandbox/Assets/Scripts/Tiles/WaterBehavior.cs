using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterBehavior : MonoBehaviour {

	//How is the water going to behave
 	[Header("Basic Water Magnitude")]
	[Space(5)]
	[Tooltip("The base Water Magnitude value. Select this if it should not be random.")]
	[Range(5,20)]
	public float waterMagnitude;
	public bool moveRight;
	

	[Header("Water Magnitude Randomization")]
	[Space(5)]
	[Tooltip("Should the Magnitude of the Water change randomly. If checked the min and max values below will be used.")]
	public bool shouldRandomize;

	[Tooltip("Select the max value of the water magnitude. This includes negative magnitude.")]
	[Range(5,20)]
	public float maxMagnitudeRange;

	[Range(1,10)]
	public float timeBetweenChange;


	private BuoyancyEffector2D buoy;
	private float time;
	// Use this for initialization
	void Start () 
	{
		buoy = this.gameObject.GetComponent<BuoyancyEffector2D>();

		if(shouldRandomize)
		{
			time = timeBetweenChange;
			SetMagnitude(GetRandomMag());
		}
		else
		{
			switch(moveRight)
			{
				case true:
				SetMagnitude(waterMagnitude);
				break;
				case false:
				waterMagnitude *= -1;
				SetMagnitude(waterMagnitude);
				break;
			}
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(shouldRandomize)
		{
			if(time <= 0)
			{
				SetMagnitude(GetRandomMag());
				time = timeBetweenChange;
			}
			else
			{
				time -= Time.deltaTime;
			}
		}
	}

	private void SetMagnitude(float mag)
	{
		Debug.Log("Current Mag: " + buoy.flowMagnitude + "  New Mag: " + mag);
		buoy.flowMagnitude = mag;
		Debug.Log("Current Mag: " + buoy.flowMagnitude);
	}

	private float GetRandomMag()
	{
		float temp = (Random.Range(0,2)*2-1) * (Random.Range(5f,maxMagnitudeRange));
		return temp;
	}

}


 

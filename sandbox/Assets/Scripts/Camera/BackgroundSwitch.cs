using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour {

	

	[System.Serializable]
	public class Backgrounds
	{
		public GameObject grouping;
		public MeshRenderer[] backImages;
		public float[] speed;
	}

	public ScrollingBackground scroll;
	public Backgrounds[] background;
	public int newSize;

public void Start()
{
	UpdateScrolling(1); //Need to change the 1 to the level they are on.
}

public void Update()
{
	if(Input.GetKeyDown(KeyCode.C)) //Debuggin need to remove
	{
		UpdateScrolling(2);
	}
	if(Input.GetKeyDown(KeyCode.P))
	{
		UpdateScrolling(1);
	} //
}
	public void UpdateScrolling(int element) //element aka level
	{
		element--;//makes it work for the array, going to change this

		newSize = background[element].backImages.Length;
		scroll.ScrollChange(background[element].grouping, newSize, background[element].backImages, background[element].speed);
	}
}

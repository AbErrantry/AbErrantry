using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour {

	

	[System.Serializable]
	public class Backgrounds
	{
		public MeshRenderer[] backImages;
		public float[] speed;
	}

	public ScrollingBackground scroll;
	public Backgrounds[] background;
	public int newSize;


	public void UpdateScrolling(int element) //element aka level
	{
		element--;//make it work for the array, going to change this

		newSize = background[element].backImages.Length;
		scroll.ScrollChange(newSize, background[element].backImages, background[element].speed);
	}
}

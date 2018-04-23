using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearDeadTrigger : MonoBehaviour
{

	public GameObject bear;
	public FollowTarget followTarget;
	public Transform player;
	public void OnTriggerEnter2D(Collider2D col)
	{
		//Debug.Log(col.gameObject.name + "        " + bear.name);
		if (bear != null && col.gameObject.name == bear.name)
		{
			//Debug.Log("Entered");
			followTarget = GameObject.Find("CameraTarget").GetComponent<FollowTarget>();
			player = GameObject.Find("Knight").GetComponent<Transform>();

			followTarget.target = player;
		}
	}
}

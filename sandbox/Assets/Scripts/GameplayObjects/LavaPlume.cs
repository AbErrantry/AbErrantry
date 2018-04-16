using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPlume : MonoBehaviour {

	public float delay;
	private float time;
	public bool requiresInput;

	// Use this for initialization
	void Start ()
	{
		time = delay;
		gameObject.GetComponent<Animator>().SetFloat("Delay", time);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!requiresInput)
		{
			if(time<=0)
			{
				gameObject.GetComponent<Animator>().SetFloat("Delay", time);
				time = delay;
				
			}
			else
			{
				time -= Time.deltaTime;
				gameObject.GetComponent<Animator>().SetFloat("Delay", time);
			}
		}
	}

	//Only used for BossPlumes
	public void PlumeIt()
	{
		gameObject.GetComponent<Animator>().SetBool("PlumeUp", true);
	}

	public void UnPlumeIt()
	{
		gameObject.GetComponent<Animator>().SetBool("PlumeUp", false);
	}


}

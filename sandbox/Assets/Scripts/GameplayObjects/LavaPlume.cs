using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPlume : MonoBehaviour {

	public float delay;
	private float time;
	public bool requiresInput;
	private bool pluming;
	private Animator anim;
	// Use this for initialization
	void Start ()
	{
		time = delay;
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!requiresInput)
		{
			if(time<=0 && !pluming)
			{
				StartCoroutine(GenericPlume());
				time = delay;
				
			}
			else
			{
				time -= Time.deltaTime;
				//gameObject.GetComponent<Animator>().SetFloat("Delay", time);
			}
		}
	}
	public IEnumerator GenericPlume()
	{
		pluming= true;
		anim.SetBool("StayDown",false);
		yield return new WaitForSeconds(delay/2);
		anim.SetBool("StayUp", false);
		anim.SetBool("StayDown",true);
		yield return new WaitForSeconds(delay/2);
		
		anim.SetBool("StayUp", true);

		pluming = false;
		StopAllCoroutines();
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

	public void PlumeFake()
	{
		gameObject.GetComponent<Animator>().Play("PlumeTell");
	}


}

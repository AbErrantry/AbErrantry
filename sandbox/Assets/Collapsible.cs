using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsible : MonoBehaviour 
{
	private Animator animator;
	private float collapseDelay;
	private float collapseTime;
	private bool isCollapsing;
	private bool collapseStarted;

	private void Start()
	{
		animator = GetComponent<Animator>();
		collapseDelay = 0.5f;
		collapseTime = 5.0f;
		isCollapsing = false;
		collapseStarted = false;
	}

	private IEnumerator Collapse()
	{
		collapseStarted = true;
		yield return new WaitForSeconds(collapseDelay);
		isCollapsing = true;
		animator.SetBool("isCollapsing", isCollapsing);
		yield return new WaitForSeconds(collapseTime);
		isCollapsing = false;
		animator.SetBool("isCollapsing", isCollapsing);
		collapseStarted = false;
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Player" && !collapseStarted)
		{
			StartCoroutine(Collapse());
		}
	}
}

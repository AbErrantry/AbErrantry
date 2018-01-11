using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{
    private float collectTime;

	//used for initialization
	private new void Start ()
    {
        base.Start();
        collectTime = 1.0f;
    }

    public IEnumerator Collect(GameObject player)
    {
        float currentTime = Time.time;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        while(Time.time - currentTime < collectTime)
        {
            transform.localScale = transform.localScale * 0.95f;

            transform.position = Vector3.Lerp(transform.position, player.transform.position, (Time.time - currentTime) / collectTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class DrowningforBoss : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D coll)
    {
		if(coll.gameObject.GetComponent<Player>() != null)
		{
			Attackable player = coll.gameObject.GetComponent<Attackable>();
			
			if(!player.isDying)
			{
        		player.Kill();
			}
		}
    }
}
}

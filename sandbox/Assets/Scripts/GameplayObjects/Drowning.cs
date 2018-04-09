using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class Drowning : MonoBehaviour {


	private void OnTriggerEnter2D(Collider2D coll)
    {
		if(coll.gameObject.GetComponent<Player>() != null || coll.gameObject.GetComponent<Enemy>() != null)
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

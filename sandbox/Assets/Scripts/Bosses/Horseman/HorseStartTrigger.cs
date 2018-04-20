using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class HorseStartTrigger : MonoBehaviour {

	public GameObject Horse;
	public LavaPlume[] plumes;
	public LavaPlume[] bigPlumes;
	public Platform leftPlatform;
	public Platform rightPlatform;
	// Use this for initialization
	public void OnTriggerEnter2D(Collider2D col)
	{
		bool bossDefeated = GameData.data.saveData.ReadBossState(Horse.name);
		if(col.gameObject.GetComponent<Player>() != null && !bossDefeated)
		{
			GameObject clone = Instantiate(Horse, new Vector3( GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.min.y, Horse.transform.position.z)
										,Quaternion.identity);
			clone.GetComponent<Horseman>().plumes = plumes;
			clone.GetComponent<Horseman>().bigPlumes = bigPlumes;
			clone.GetComponent<Horseman>().leftPlatform = leftPlatform;
			clone.GetComponent<Horseman>().rightPlatform = rightPlatform;

			Destroy(this.gameObject);
		}
	}
}
}

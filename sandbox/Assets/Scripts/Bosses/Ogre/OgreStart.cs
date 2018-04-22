using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class OgreStart : MonoBehaviour {

	
	public GameObject ogrePrefab;
	public Collider2D posPositions;
	void Start () 
	{
		
	}
	public void OnTriggerEnter2D(Collider2D col)
	{
		bool bossDefeated = GameData.data.saveData.ReadBossState(ogrePrefab.name);
		if(col.gameObject.GetComponent<Player>() != null && !bossDefeated)
		{
			GameObject clone = Instantiate(ogrePrefab, new Vector3(gameObject.GetComponent<BoxCollider2D>().bounds.max.x, gameObject.GetComponent<BoxCollider2D>().bounds.center.y, ogrePrefab.transform.position.z)
										,Quaternion.identity);

			clone.GetComponent<OgreBoss>().posPositions = posPositions;
			Destroy(this.gameObject);
		}
	}
}
}

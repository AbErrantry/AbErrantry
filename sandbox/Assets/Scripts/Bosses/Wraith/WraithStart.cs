using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class WraithStart : MonoBehaviour 
	{
		public Collider2D rFTrigger;
		public GameObject wraithPrefab;

		void Start () 
		{
			
		}
		public void OnTriggerEnter2D(Collider2D col)
		{
			bool bossDefeated = GameData.data.saveData.ReadBossState(wraithPrefab.name);
			if(col.gameObject.GetComponent<Player>() != null && !bossDefeated)
			{
				GameObject clone = Instantiate(wraithPrefab, new Vector3( GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.min.y, wraithPrefab.transform.position.z)
											,Quaternion.identity);

				clone.GetComponent<WraithBoss>().rainFireTrigger = rFTrigger;

				Destroy(this.gameObject);
			}
		}
	}
}

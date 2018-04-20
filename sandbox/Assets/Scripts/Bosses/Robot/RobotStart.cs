using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class RobotStart : MonoBehaviour
	{

		public GameObject robotPrefab;

		void Start()
		{

		}
		public void OnTriggerEnter2D(Collider2D col)
		{
			bool bossDefeated = GameData.data.saveData.ReadBossState(robotPrefab.name);
			if(col.gameObject.GetComponent<Player>() != null && !bossDefeated)
			{
				Instantiate(robotPrefab, new Vector3(GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.min.y, robotPrefab.transform.position.z), Quaternion.identity);

				Destroy(this.gameObject);
			}
		}
	}
}

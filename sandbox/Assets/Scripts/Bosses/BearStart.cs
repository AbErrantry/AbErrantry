using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class BearStart : MonoBehaviour
	{

		public GameObject bear;
		public DestroyAttackablesUnlockAction destDoor;
		public FollowTarget followTarget;
		public Transform spawnPOS;
		public EnemyMovement movement;
		public GameObject tilemap;
		public BearDeadTrigger bearDead;

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (!GameData.data.saveData.ReadBossState("Bear"))
			{
				if (collision.gameObject.name == "Knight")
				{
					followTarget = GameObject.Find("CameraTarget").GetComponent<FollowTarget>();
					GameObject clone = Instantiate(bear, spawnPOS.position, Quaternion.identity);
					clone.GetComponentInChildren<TileDestroyer>().tilemapGameObject = tilemap;
					followTarget.target = clone.transform;
					bearDead.bear = clone;
					Destroy(this.gameObject);
				}
			}
		}
	}
}

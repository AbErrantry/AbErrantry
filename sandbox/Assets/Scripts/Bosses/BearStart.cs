using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class BearStart : MonoBehaviour {

    public GameObject bear;
	public DestroyAttackablesUnlockAction destDoor;
	public FollowTarget followTarget;
	public Transform spawnPOS;
	public EnemyMovement movement;
	public GameObject endBearcon;
	public GameObject tilemap;


    public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.name == "Knight")
		{
			followTarget = GameObject.Find("CameraTarget").GetComponent<FollowTarget>();
			GameObject clone = Instantiate(bear, spawnPOS.position, Quaternion.identity);
			clone.GetComponentInChildren<TileDestroyer>().tilemapGameObject = tilemap;
			clone.GetComponent<Enemy>().beacCon.beacons[0] = endBearcon;
			destDoor.attackables[0] = clone.GetComponent<Attackable>();
			followTarget.target = clone.transform;
			Destroy(this.gameObject);
		}
	}
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class BearStart : MonoBehaviour {

    public GameObject bear;
	public Transform spawnPOS;
	public EnemyMovement movement;
	public GameObject endBearcon;
	public GameObject tilemap;

    public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.name == "Knight")
		{
			GameObject clone = Instantiate(bear, spawnPOS.position, Quaternion.identity);
			clone.GetComponentInChildren<TileDestroyer>().tilemapGameObject = tilemap;
			clone.GetComponent<Enemy>().beacCon.beacons[0] = endBearcon;
			
			Destroy(this.gameObject);
		}
	}
}
}

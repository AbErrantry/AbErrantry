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
		if(col.gameObject.GetComponent<Player>() != null)
		{
			GameObject clone = Instantiate(ogrePrefab, new Vector3(GetComponent<BoxCollider2D>().bounds.max.x, GetComponent<BoxCollider2D>().bounds.min.y, ogrePrefab.transform.position.z)
										,Quaternion.identity);

			clone.GetComponent<OgreBoss>().posPositions = posPositions;
			Destroy(this.gameObject);
		}
	}
}
}

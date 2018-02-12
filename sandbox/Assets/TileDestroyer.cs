using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour {

    public GameObject tilemapGameObject;
	public LayerMask attackables;
    Tilemap tilemap;
 
    void Start()
    {
        if (tilemapGameObject != null)
        {
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }
 
    void OnCollisionStay2D(Collision2D collision)
    {
        

        if (tilemap != null && tilemapGameObject == collision.gameObject)
        {
           StartCoroutine(BreakEm(collision));
        }
		else
		{
			Destroy(collision.gameObject);
		}
    }

	 public void OnCollisionEnter2D(Collision2D collision)
  {
      if (collision.gameObject.tag == "Interactable")
      {
          Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
      }
  }

	public IEnumerator BreakEm(Collision2D collision)
	{
		Vector3 hitPosition = Vector3.zero;
		 foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }

			yield return null;
	}
}

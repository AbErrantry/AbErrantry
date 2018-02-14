using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Character2D
{
    public class TileDestroyer : MonoBehaviour
    {

        public EnemyAttack enemyAttack;
        public GameObject tilemapGameObject;
        Tilemap tilemap;

        public GameObject explodingTile;
        public Material material;

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
                //StartCoroutine(enemy.Attack());
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Interactable")
            {
                Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
            }
            else if (collision.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
                collision.gameObject.GetComponent<Player>().TakeDamage(gameObject, 100.0f);
                enemyAttack.PlayAttack();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
            }
        }

        public IEnumerator BreakEm(Collision2D collision)
        {
            enemyAttack.PlayAttack();
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
            GameObject tempExplode = Instantiate(explodingTile, hitPosition, Quaternion.identity) as GameObject;
            tempExplode.GetComponent<SpriteRenderer>().material = material;
            yield return new WaitForSeconds(0.3f);
            Destroy(tempExplode);
        }
    }
}

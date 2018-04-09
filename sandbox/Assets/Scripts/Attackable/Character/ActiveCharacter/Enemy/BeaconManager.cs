using UnityEngine;

namespace Character2D
{
    public class BeaconManager : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.gameObject.layer == 9) //9 is the enemy layer
            {
               Enemy enemy = collision.gameObject.GetComponent<Enemy>();

               if( enemy.beacCon.currTarget == this.gameObject && !enemy.chasingPlayer)
               {
                collision.gameObject.GetComponent<Enemy>().SwitchBeacon();
               }
            }
        }
    }
}

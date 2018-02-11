using UnityEngine;

namespace Character2D
{
    public class BeaconManager : MonoBehaviour
    {
        public Enemy enemy;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == enemy.name && enemy.beacCon.currTarget == this.gameObject && !enemy.chasingPlayer)
            {
                enemy.SwitchBeacon();
            }
        }
    }
}

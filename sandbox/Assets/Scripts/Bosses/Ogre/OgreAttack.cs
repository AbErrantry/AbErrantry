using UnityEngine;

namespace Character2D
{
	public class OgreAttack : MonoBehaviour
	{
		public OgreBoss ogre;

		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Player>() != null)
			{
				ogre.ApplyDamage(col.gameObject);
			}
		}
	}
}

using UnityEngine;

namespace Character2D
{
	public class OgreAttack : MonoBehaviour
	{
		public OgreBoss ogre;

		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Attackable>() != null)
			{
				ogre.ApplyDamage(col.gameObject);
			}
		}
	}
}

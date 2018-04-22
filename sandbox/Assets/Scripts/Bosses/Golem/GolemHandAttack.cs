using UnityEngine;

namespace Character2D
{
	public class GolemHandAttack : MonoBehaviour
	{

		public GolemBoss golem;
		public void Start()
		{
			golem = GetComponentInParent<GolemBoss>();
		}
		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Player>() != null)
			{
				golem.ApplyDamage(col.gameObject);
			}
		}
	}
}

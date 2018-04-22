using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class RobotAttack : MonoBehaviour 
	{

		public RobotBoss robot;

		public void Start()
		{
			robot = gameObject.GetComponentInParent<RobotBoss>();
		}
		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Player>() != null)
			{
				robot.ApplyDamage(col.gameObject);
			}
		}
	}
}

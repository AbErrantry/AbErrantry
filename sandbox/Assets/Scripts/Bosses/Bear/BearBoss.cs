using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class BearBoss : Boss
	{
		private BearMovement bearMovement;

		private FMOD.Studio.EventInstance bearMusic;

		//used for initialization
		protected new void Start()
		{
			name = "Bear";
			base.Start();
			bearMovement = GetComponent<BearMovement>();
			canFlinch = false;
			canKnockBack = false;
			canTakeDamage = true;

			BackgroundSwitch.instance.ResetSongs();

			bearMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/boss/plains_boss");
			bearMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			bearMusic.start();
		}

		private new void Update()
		{
			bearMovement.mvmtSpeed = 1.0f;
		}

		protected override void InitializeDeath()
		{
			//take away enemy input
			//enemy no longer targets player
			//enemy no longer attackable
			isDying = true;
			anim.SetBool("isDying", isDying); //death animation
		}

		public override void FinalizeDeath()
		{
			BossDefeated();
			//TODO: give random amount of gold around enemy difficulty.
			Destroy(gameObject);
		}

		private void OnDestroy()
		{
			if (GameObject.Find("CameraTarget"))
			{
				var followTarget = GameObject.Find("CameraTarget").GetComponent<FollowTarget>();
				if (followTarget != null)
				{
					followTarget.target = Player.instance.gameObject.transform;
				}
			}
			bearMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}

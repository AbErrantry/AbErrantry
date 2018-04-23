using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class PrincessImageSwapper : MonoBehaviour
{
	void Start()
	{
		if (Player.instance.isSavingPrincess)
		{
			gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Characters/Princess_AnimationOverride");
		}
		else
		{
			gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Characters/Prince_AnimationOverride");
		}
	}
}

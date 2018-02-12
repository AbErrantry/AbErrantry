using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateGenerator : MonoBehaviour
{
	public List<GameObject> crates;
	private List<Vector2> initialCrateLocations;
	private WorldButton worldButton;
	public GameObject crate;
	public Material material;

	//used for initialization
	private void Start()
	{
		initialCrateLocations = new List<Vector2>();
		foreach (GameObject crate in crates)
		{
			initialCrateLocations.Add(crate.transform.position);
		}
	}

	private void OnEnable()
	{
		worldButton = GetComponent<WorldButton>();
		worldButton.OnButtonPressed += RespawnCrates;
	}

	private void OnDisable()
	{
		worldButton.OnButtonPressed -= RespawnCrates;
	}

	private void RespawnCrates()
	{
		for (int i = crates.Count - 1; i >= 0; i--)
		{
			if (crates[i] == null)
			{
				crates.RemoveAt(i);
			}
			else
			{
				crates[i].GetComponent<Character2D.Crate>().DestroyCrate();
				crates.RemoveAt(i);
			}
		}
		foreach (Vector2 crateLocation in initialCrateLocations)
		{
			GameObject newCrate = Instantiate(crate, crateLocation, Quaternion.identity)as GameObject;
			newCrate.GetComponent<SpriteRenderer>().material = material;
			crates.Add(newCrate);
		}
	}
}

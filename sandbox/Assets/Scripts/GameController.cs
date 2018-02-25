using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private RootSystems _systems;

    // Use this for initialization
    private void Start()
	{
	    var contexts = Contexts.sharedInstance;

        _systems = new RootSystems(contexts);

	    _systems.Initialize();
    }

    private void Update()
    {
        _systems.Execute();
    }
}

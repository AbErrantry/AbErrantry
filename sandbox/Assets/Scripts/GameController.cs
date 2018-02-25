using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameSystems _systems;

    // Use this for initialization
    private void Start()
	{
	    var contexts = Contexts.sharedInstance;

        _systems = new GameSystems(contexts);

	    _systems.Initialize();
    }

    private void Update()
    {
        _systems.Execute();
    }
}

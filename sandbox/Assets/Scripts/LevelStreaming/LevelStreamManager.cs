using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStreamManager : MonoBehaviour
{
    public static List<string> scenes;

    public event Action OnRefreshComplete;

    public string sceneName; //the name of the scene to be loaded/unloaded

    private Scene scene; //an object reference to the scene
    private AsyncOperation asyncLoad; //async operation to load the scene
    private AsyncOperation asyncUnload; //async operation to unload the scene
    public Character2D.PlayerInput playerInput;

    private bool isLoading;

    //used for initialization
    private void Start()
    {
        scenes = new List<string>()
        {
            "Area1-AP",
            "Persistent-SC"
        };

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        //find and set the scene using its name
        scene = SceneManager.GetSceneByName(sceneName);

        isLoading = false;
    }

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player,
        if (other.tag == "Player")
        {
            //if the scene is not loaded yet, make the player wait for loading
            if (!IsLoaded()&& isLoading)
            {
                StartCoroutine(WaitUntilLoaded());
            }
            else if (IsLoaded()&& isLoading)
            {
                isLoading = false;
                Debug.Log("Scene " + sceneName + " was loaded.");
            }
            else if (!IsLoaded()&& !isLoading)
            {
                Debug.LogError("got here.");
                LoadScene();
                StartCoroutine(WaitUntilLoaded());
            }
        }
    }

    public void MakeActive()
    {
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    private IEnumerator WaitUntilLoaded()
    {

        playerInput.DisableInput();
        playerInput.ToggleLoadingContainer(true);
        while (!IsLoaded())
        {
            yield return new WaitForFixedUpdate();
        }
        playerInput.ToggleLoadingContainer(false);
        playerInput.EnableInput();
    }

    //coroutine that loads a scene asynchronously and additively
    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        Debug.Log(sceneName + " is being loaded.");
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true; //scene will be activated automatically
        while (!asyncLoad.isDone)
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(sceneName + " was loaded.");
        scenes.Add(sceneName);
    }

    //coroutine that unloads a scene asynchronously
    private IEnumerator UnloadSceneAsync()
    {
        Debug.Log(sceneName + " is being unloaded.");
        asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(sceneName + " was unloaded.");
        scenes.Remove(sceneName);
    }

    //function that starts the coroutine to load a scene
    public void LoadScene()
    {
        if (!IsLoaded())
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    //function that starts the coroutine to unload a scene
    public void UnloadScene()
    {
        if (IsLoaded())
        {
            StartCoroutine(UnloadSceneAsync());
        }
    }

    public bool IsLoaded()
    {
        if (scenes.Contains(sceneName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RefreshLevels()
    {
        UnloadScene();
        StartCoroutine(WaitForUnload());
    }

    private IEnumerator WaitForUnload()
    {
        while (IsLoaded())
        {
            yield return new WaitForFixedUpdate();
        }
        LoadScene();
        StartCoroutine(WaitForLoad());
    }

    private IEnumerator WaitForLoad()
    {
        while (!IsLoaded())
        {
            yield return new WaitForFixedUpdate();;
        }
        OnRefreshComplete();
    }

}

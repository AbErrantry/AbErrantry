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
        }
    }

    public void MakeActive()
    {
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    private IEnumerator WaitUntilLoaded()
    {
        playerInput.DisableInput(false);
        playerInput.ToggleLoadingContainer(true);
        Debug.LogError("Loading... add UI for loading."); //TODO: add UI popup for loading
        while (!IsLoaded())
        {
            yield return null;
        }
        playerInput.ToggleLoadingContainer(false);
        playerInput.EnableInput();
    }

    //coroutine that loads a scene asynchronously and additively
    private IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true; //scene will be activated automatically
        while (!asyncLoad.isDone)
        {
            yield return asyncLoad;
        }
        scenes.Add(sceneName);
    }

    //coroutine that unloads a scene asynchronously
    private IEnumerator UnloadSceneAsync()
    {
        asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        scenes.Remove(sceneName);
    }

    //function that starts the coroutine to load a scene
    public void LoadScene()
    {
        if (!IsLoaded()&& !isLoading)
        {
            isLoading = true;
            StopAllCoroutines();
            StartCoroutine(LoadSceneAsync());
        }
    }

    //function that starts the coroutine to unload a scene
    public void UnloadScene()
    {
        if (IsLoaded())
        {
            StopAllCoroutines();
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
            yield return null;
        }
        LoadScene();
        StartCoroutine(WaitForLoad());
    }

    private IEnumerator WaitForLoad()
    {
        while (!IsLoaded())
        {
            yield return null;
        }
        OnRefreshComplete();
    }

}

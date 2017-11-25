using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStreamManager : MonoBehaviour
{
    public string sceneName;

    private AsyncOperation asyncLoad;
    private AsyncOperation asyncUnload;

    public bool isLoaded;
    public Scene scene;

    //used for initialization
    void Start()
    {
        scene = SceneManager.GetSceneByName(sceneName);
        isLoaded = scene.isLoaded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Knight")
        {
            if(!isLoaded)
            {
                //TODO: add input reference here to disable input until level is loaded
                //maybe a delegate to the AsyncOperation.complete
                    //when it fires, reenable input. 
                //edge case for low-end systems
                Debug.Log("Error. " + sceneName + " is not loaded yet. Disable input and wait until loaded." + isLoaded);
            }
        }
    }

    IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        isLoaded = true;
    }

    IEnumerator UnloadSceneAsync()
    {
        asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        isLoaded = false;
    }

    public void LoadScene()
    {
        if (!isLoaded)
        {
            StopAllCoroutines();
            StartCoroutine(LoadSceneAsync());
        }
    }

    public void UnloadScene()
    {
        if (isLoaded)
        {
            StopAllCoroutines();
            StartCoroutine(UnloadSceneAsync());
        }
    }
}

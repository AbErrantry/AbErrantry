using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStreamManager : MonoBehaviour
{
    public string sceneName; //the name of the scene to be loaded/unloaded
    private bool isLoaded; //whether the scene is loaded or not
    private Scene scene; //an object reference to the scene
    private AsyncOperation asyncLoad; //async operation to load the scene
    private AsyncOperation asyncUnload; //async operation to unload the scene
    public Character2D.PlayerInput playerInput;

    //used for initialization
    private void Start()
    {
        //find and set the scene using its name
        scene = SceneManager.GetSceneByName(sceneName);

        //initialize whether the scene is loaded using the Scene member function
        isLoaded = scene.isLoaded;
    }

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player,
        if (other.tag == "Player")
        {
            //if the scene is not loaded yet, make the player wait for loading
            if (!isLoaded)
            {
                WaitUntilLoaded();
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
        Debug.LogError("Loading... add UI for loading."); //TODO: add UI popup for loading
        while (!isLoaded)
        {
            yield return null;
        }
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
        isLoaded = true;
    }

    //coroutine that unloads a scene asynchronously
    private IEnumerator UnloadSceneAsync()
    {
        asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        isLoaded = false;
    }

    //function that starts the coroutine to load a scene
    public void LoadScene()
    {
        if (!isLoaded)
        {
            StopAllCoroutines();
            StartCoroutine(LoadSceneAsync());
        }
    }

    //function that starts the coroutine to unload a scene
    public void UnloadScene()
    {
        if (isLoaded)
        {
            StopAllCoroutines();
            StartCoroutine(UnloadSceneAsync());
        }
    }
}

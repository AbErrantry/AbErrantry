using System;
using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStreamManager : MonoBehaviour
{
    private static List<string> scenes; // Global list of the active scenes.

    public event Action OnRefreshComplete; // Event that fires when this scene completed refresing.

    public LevelInfo levelInfo;

    public string sceneName; // The name of the scene set in the hierarchy.
    private Scene scene; // An object reference to the scene
    private AsyncOperation asyncLoad; // Async operation to load the scene
    private AsyncOperation asyncUnload; // Async operation to unload the scene
    private bool isLoading; // Whether the scene is loading or not.

    private PlayerInventory playerInventory;

    // Used for initialization.
    private void Start()
    {
        //TODO: set only the persistent scene as the active scene from the main menu.
        //       this is a temporary fix for testing since we never technically load into Area1-AP.
        if (scenes == null)
        {
            InitializeActiveScenes();
        }

        sceneName = levelInfo.name;

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        //find and set the scene using its name
        scene = SceneManager.GetSceneByName(sceneName);
        isLoading = false;

        playerInventory = PlayerInput.instance.gameObject.GetComponent<PlayerInventory>();
    }

    public static void InitializeActiveScenes()
    {
        scenes = new List<string>()
        {
            "Persistent-SC"
        };
    }

    // Fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the colliding object is the player, check to see where they are at with loading.
        if (other.tag == "Player")
        {
            if (!IsLoaded() && isLoading) // Scene is still loading. Make the player wait.
            {
                StartCoroutine(WaitUntilLoaded());
            }
            else if (IsLoaded() && isLoading) // Scene is loaded.
            {
                isLoading = false;
            }
            else if (!IsLoaded() && !isLoading) // Scene has not even started loading for some reason.
            {
                Debug.LogError("Got to LevelStreamManager for " + sceneName +
                    " without level being loaded or beginning to be loaded.");
                LoadScene();
                StartCoroutine(WaitUntilLoaded());
            }
        }
    }

    // Makes the current scene the active scene for things like object instantiation.
    public void MakeActive()
    {
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    // Coroutine that disables player input until the level is loaded.
    private IEnumerator WaitUntilLoaded()
    {
        PlayerInput.instance.DisableInput();
        PlayerInput.instance.ToggleLoadingContainer(true);
        while (!IsLoaded())
        {
            yield return new WaitForFixedUpdate();
        }
        isLoading = false;
        PlayerInput.instance.ToggleLoadingContainer(false);
        PlayerInput.instance.EnableInput();
    }

    // Coroutine that loads the level asynchronously.
    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return asyncLoad;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        LoadItems();
        LoadCharacters();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes[0]));
        scenes.Add(sceneName);
    }

    public void LoadPersistentData()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes[0]));
        LoadItems();
        LoadCharacters();
    }

    private void LoadItems()
    {
        var levelItems = new List<LevelItemTuple>();
        levelItems = GameData.data.saveData.ReadLooseItems(sceneName);
        foreach (LevelItemTuple item in levelItems)
        {
            playerInventory.InstantiateItem(GameData.data.itemData.itemDictionary[item.name], new Vector3(item.xLoc, item.yLoc, 0.0f), false, item.id);
        }
    }

    private void LoadCharacters()
    {
        var levelCharacters = new List<CharacterInfoTuple>();
        levelCharacters = GameData.data.saveData.ReadCharacterInfo(sceneName);
        foreach (var character in levelCharacters)
        {
            CharacterManager.instance.SpawnCharacter(character);
        }
    }

    // Coroutine that unloads the level asynchronously.
    private IEnumerator UnloadSceneAsync()
    {
        asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        yield return asyncUnload;
        scenes.Remove(sceneName);
    }

    // Invokes the loading of the level if not already loaded.
    public void LoadScene()
    {
        if (!IsLoaded())
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    // Invokes the loading of the level if still loaded.
    public void UnloadScene()
    {
        if (IsLoaded())
        {
            StartCoroutine(UnloadSceneAsync());
        }
    }

    // Checks to see if the level is loaded or not.
    private bool IsLoaded()
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

    // Begins the process of unloading and loading the level.
    public void RefreshLevel()
    {
        UnloadScene();
        StartCoroutine(WaitForRefreshUnload());
    }

    // Begins the process of unloading and loading the level.
    public void FlushLevel()
    {
        UnloadScene();
    }

    // Coroutine that waits until the level is unloaded to reload it.
    private IEnumerator WaitForRefreshUnload()
    {
        while (IsLoaded())
        {
            yield return new WaitForFixedUpdate();
        }
        LoadScene();
        StartCoroutine(WaitForRefreshLoad());
    }

    // Coroutine that reloads the level then triggers the refresh completion event.
    private IEnumerator WaitForRefreshLoad()
    {
        while (!IsLoaded())
        {
            yield return new WaitForFixedUpdate();
        }
        OnRefreshComplete();
    }
}

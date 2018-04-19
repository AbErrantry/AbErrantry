using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitchMenu : MonoBehaviour
{
    public static BackgroundSwitchMenu instance; // Singleton

    [System.Serializable]
    public class Backgrounds
    {
        public GameObject grouping;
        public MeshRenderer[] backImages;
        public float[] speed;
    }

    public ScrollingBackgroundMenu scroll;
    public Backgrounds[] background;
    public int newSize;

    private FMOD.Studio.EventInstance plainsMusic;
    private FMOD.Studio.EventInstance castleMusic;
    private FMOD.Studio.EventInstance caveMusic;
    private FMOD.Studio.EventInstance forestMusic;
    private FMOD.Studio.EventInstance spookyMusic;
    private FMOD.Studio.EventInstance iceMusic;
    private FMOD.Studio.EventInstance lavaMusic;

    private List<FMOD.Studio.EventInstance> songs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        plainsMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/plains");
        castleMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/castle");
        caveMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/cave");
        forestMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/forest");
        spookyMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/spooky");
        iceMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/ice");
        lavaMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/lava");

        plainsMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        castleMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        caveMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        forestMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        spookyMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        iceMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        lavaMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        songs = new List<FMOD.Studio.EventInstance>()
        {
            plainsMusic,
            castleMusic,
            caveMusic,
            forestMusic,
            spookyMusic,
            iceMusic,
            lavaMusic
        };

        int randomBackground = Random.Range(1, 7);

        UpdateScrolling(randomBackground); //TODO: Need to change the 1 to the level they are on.
    }

    public void UpdateScrolling(int element) //element aka level
    {
        ResetSong();
        if (element == 0)
        {
            return;
        }
        element--; //makes it work for the array, going to change this
        SetSong(element);
        newSize = background[element].backImages.Length;
        scroll.ScrollChange(background[element].grouping, newSize, background[element].backImages, background[element].speed);
    }

    public void SetSong(int index)
    {
        songs[index].start();
    }

    public void ResetSong()
    {
        foreach (var song in songs)
        {
            song.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour
{
    public static BackgroundSwitch instance; // Singleton

    [System.Serializable]
    public class Backgrounds
    {
        public GameObject grouping;
        public MeshRenderer[] backImages;
        public float[] speed;
    }

    public ScrollingBackground scroll;
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

    private int currentElement;

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

        currentElement = 0;

        UpdateScrolling(0);
    }

    public void UpdateScrolling(int element) //element aka level
    {
        if (currentElement == element)
        {
            return;
        }
        StopSongs();
        if (element == 0)
        {
            return;
        }
        currentElement = element;
        element--;
        SetSong(element);
        newSize = background[element].backImages.Length;
        scroll.ScrollChange(background[element].grouping, newSize, background[element].backImages, background[element].speed);
    }

    public void SetSong(int index)
    {
        songs[index].start();
    }

    public void StopSongs()
    {
        foreach (var song in songs)
        {
            song.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void ResetSongs()
    {
        currentElement = -1;
        foreach (var song in songs)
        {
            song.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

}

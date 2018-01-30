﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraphicsOptions : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => LoadGame());
    }

    // Update is called once per frame
    void LoadGame()
    {
        SceneManager.LoadScene("GraphicsOptions");
    }
}
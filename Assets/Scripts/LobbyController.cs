using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        playButton.onClick.AddListener(Quit);

    }

    private void Quit()
    {
        Application.Quit();
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

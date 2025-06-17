using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("UI Reference")]
    public Button restartButton;
    public Button lobbyButton;
    public GameObject gameOverPanel;


    private int currentSceneIndex;

    private void Awake()
    {
        restartButton.onClick.AddListener(ReloadLevel);
        lobbyButton.onClick.AddListener(loadLobby);
    }


    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDied()
    {
        Debug.Log("Canvas is getting activated.");
        gameOverPanel.SetActive(true);
    }


    public void ReloadLevel()
    {
        gameOverPanel.SetActive(false);
        Debug.Log("CurrentSceneIndex " + currentSceneIndex);
        SceneManager.LoadScene(currentSceneIndex);
    }


    private void loadLobby()
    {
        SceneManager.LoadScene(0);
    }
}

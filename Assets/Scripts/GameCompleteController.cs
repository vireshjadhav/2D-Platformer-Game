using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCompleteController : MonoBehaviour
{
    [Header("Game Complete:UI Reference")]
    public Button quitButton;
    public Button lobbyButton;
    public GameObject gameCompletePanel;

    private int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void Awake()
    {
        quitButton.onClick.AddListener(QuitLevel);
        lobbyButton.onClick.AddListener(loadLobby);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null) {
            gameCompletePanel.SetActive(true);
        }
    }

    public void QuitLevel()
    {
        Application.Quit();
    }


    private void loadLobby()
    {
        SceneManager.LoadScene(0);
    }

}

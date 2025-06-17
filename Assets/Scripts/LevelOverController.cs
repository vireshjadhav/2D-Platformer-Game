using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    [SerializeField] private int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Level finished by the player");
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = currentSceneIndex + 1;
        int totalNumberOfScene = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex < totalNumberOfScene)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ReloadLevel()
    {
        Debug.Log("CurrentSceneIndex " + currentSceneIndex);
        int previousSceneIndex = currentSceneIndex - 1;
        SceneManager.LoadScene(previousSceneIndex);
    }
}

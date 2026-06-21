using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("UI Reference")]
    public Button restartButton;
    public Button lobbyButton;
    public GameObject gameOverPanel;
    public TextMeshProUGUI levelText;
    public int levelNumber = 1;

    private int currentSceneIndex;

    public LivesController livesController;
    public Transform respawnPoint;

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
        UpdateLeveName();
    }

    private void UpdateLeveName()
    {
        levelText.text = "Level: " + levelNumber;
    }

    public void PlayerDied()
    {
        SoundManager.Instance.Play(Sounds.PlayerDeath);
        gameOverPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void ReloadLevel()
    {
        gameOverPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SoundManager.Instance.StopAllSounds();

        livesController.ResetPlayer();
        livesController.playerController.transform.position = respawnPoint.position;
        SceneManager.LoadScene(currentSceneIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(RestartMusicAfterSceneLoad());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundManager.Instance.RestartMusic();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator RestartMusicAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.RestartMusic();
    }

    private void loadLobby()
    {
        SoundManager.Instance.StopAllSounds();
        SceneManager.LoadScene(0);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

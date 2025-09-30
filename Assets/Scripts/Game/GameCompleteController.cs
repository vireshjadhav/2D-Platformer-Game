using System.Collections;
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

    [SerializeField] private ParticleController confettiEffect1;
    [SerializeField] private ParticleController confettiEffect2;

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

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null) 
        {
            collision.gameObject.GetComponent<PlayerController>().SetGameWon();
            Debug.Log("Player Hit Game Complete Object");
            confettiEffect1.PlayPlayerWinEffect();    
            confettiEffect2.PlayPlayerWinEffect();    
            StartCoroutine(WaitForSeconds(3f));
        }
    }

    private IEnumerator WaitForSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameCompletePanel.SetActive(true);
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
